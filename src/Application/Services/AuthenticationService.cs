using Application.Common;
using Application.Common.Constants;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Extensions;
using Microsoft.Extensions.Logging;

namespace Application.Services;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IEmailService _emailService;
    private readonly IConfirmationTokenRepository _confirmationTokenRepository;
    private readonly IEmailConfirmationTokenProvider _emailConfirmationTokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthenticationService> _logger;
    public AuthenticationService(IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IAccessTokenService accessTokenService,
        IRefreshTokenService refreshTokenService,
        IEmailService emailService,
        IUnitOfWork unitOfWork,
        IConfirmationTokenRepository emailVerificationTokenRepository,
        ILogger<AuthenticationService> logger,
        IEmailConfirmationTokenProvider emailConfirmationTokenProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _accessTokenService = accessTokenService;
        _refreshTokenService = refreshTokenService;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _confirmationTokenRepository = emailVerificationTokenRepository;
        _logger = logger;
        _emailConfirmationTokenProvider = emailConfirmationTokenProvider;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {

        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !user.EmailVerified)
        {
            return Result<LoginResponse>.Failure(ErrorMessages.InvalidCredentials);
        }

        bool verified = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!verified)
        {
            return Result<LoginResponse>.Failure(ErrorMessages.InvalidCredentials);
        }

        var accessToken = _accessTokenService.GenerateAccessToken(user);

        var refreshToken = _refreshTokenService.GenerateRefreshToken(user.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<LoginResponse>.Success(new LoginResponse(accessToken!, refreshToken.Token));
    }

    public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {

        var storedRefreshToken = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (storedRefreshToken is null)
        {
            return Result<RefreshTokenResponse>.Failure(ErrorMessages.RefreshTokenInvalid);
        }

        if (storedRefreshToken.IsRevokedOrExpired())
        {
            return Result<RefreshTokenResponse>.Failure(ErrorMessages.RefreshTokenExpired);
        }

        var claimsPrincipal = _accessTokenService.GetPrincipalFromExpiredAccessToken(request.AccessToken);

        if (claimsPrincipal is null)
        {
            return Result<RefreshTokenResponse>.Failure(ErrorMessages.AccessTokenMalformed);
        }

        string? username = _accessTokenService.GetUsernameCliam(claimsPrincipal);

        var user = await _userRepository.GetByUsernameAsync(username!, cancellationToken);

        if (user!.Id != storedRefreshToken.UserId)
        {
            return Result<RefreshTokenResponse>.Failure(ErrorMessages.RefreshTokenUserMismatch);
        }

        _refreshTokenService.Delete(storedRefreshToken);

        string newAccessToken = _accessTokenService.GenerateAccessToken(user);

        var newRefreshToken = _refreshTokenService.GenerateRefreshToken(user!.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse(newAccessToken!, newRefreshToken.Token));
    }

    public async Task<Result<UserResponse>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _userRepository.IsEmailUniqueAsync(request.Email, cancellationToken))
        {
            return Result<UserResponse>.Failure(ErrorMessages.EmailAlreadyExists);
        }

        if (!await _userRepository.IsUsernameUniqueAsync(request.Username, cancellationToken))
        {
            return Result<UserResponse>.Failure(ErrorMessages.UsernameAlreadyExists);
        }

        string passwordHash = _passwordHasher.Hash(request.Password);

        var user = User.Create(request.Username, passwordHash, request.Email);

        var confirmationToken = EmailVerificationToken.Default;

        using var transaction = _unitOfWork.BeginTransaction();

        try
        {
            _userRepository.Insert(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            confirmationToken = EmailVerificationToken.Create(user.Id, _emailConfirmationTokenProvider.GenerateToken(), DateTime.Now, DateTime.Now.AddMinutes(2));

            _confirmationTokenRepository.Insert(confirmationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailService.SendVerificationEmailAsync(user, confirmationToken, cancellationToken);

            transaction.Commit();
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred during user registration: {0}", e.Message);

            transaction.Rollback();
            throw;
        }

        return Result<UserResponse>.Success(new UserResponse(user.Id, user.Username, user.Email, []));
    }

    public async Task<Result<string>> ResendVerificationEmailAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure($"user with id {userId} does not exist.");
        }

        var newVerificationToken = EmailVerificationToken.Create(userId, _emailConfirmationTokenProvider.GenerateToken(), createdAt: DateTime.Now, expiresAt: DateTime.Now.AddMinutes(2));

        _confirmationTokenRepository.Insert(newVerificationToken);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "an error occurred while saving a new confirmation email token.");
            throw;
        }

        await _emailService.SendVerificationEmailAsync(user, newVerificationToken, cancellationToken);

        return Result<string>.Success("a new confirmation email has been sent to your registered email address");
    }

    public async Task<Result<string>> RevokeRefreshTokenAsync(RevokeRefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var tokenToRevoke = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (tokenToRevoke is null)
        {
            return Result<string>.Failure(ErrorMessages.RefreshTokenNotFound);
        }

        tokenToRevoke.Revoke();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success("Refresh token has been revoked successfully.");
    }

    public async Task<bool> VerifyEmailAsync(string confirmationToken, CancellationToken cancellationToken = default)
    {

        var storedToken = await _confirmationTokenRepository.GetAsync(confirmationToken, cancellationToken);

        if (storedToken is null || storedToken.ExpiresAt < DateTime.Now || storedToken.User.EmailVerified)
        {
            return false;
        }

        storedToken.User.SetEmailVerified(true);

        _confirmationTokenRepository.Delete(storedToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
