using Application.Common;
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
    private readonly IEmailVerificationTokenRepository _verificationTokenRepository;
    private readonly IVerificationTokenGenerator _verificationTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthenticationService> _logger;
    public AuthenticationService(IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IAccessTokenService accessTokenService,
        IRefreshTokenService refreshTokenService,
        IEmailService emailService,
        IUnitOfWork unitOfWork,
        IEmailVerificationTokenRepository emailVerificationTokenRepository,
        ILogger<AuthenticationService> logger,
        IVerificationTokenGenerator verificationTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _accessTokenService = accessTokenService;
        _refreshTokenService = refreshTokenService;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _verificationTokenRepository = emailVerificationTokenRepository;
        _logger = logger;
        _verificationTokenGenerator = verificationTokenGenerator;
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

        var verificationToken = EmailVerificationToken.Default;

        using var transaction = _unitOfWork.BeginTransaction();

        try
        {
            _userRepository.Insert(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            verificationToken = EmailVerificationToken.Create(user.Id, _verificationTokenGenerator.GenerateToken(), DateTime.Now, DateTime.Now.AddMinutes(15));

            _verificationTokenRepository.Insert(verificationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailService.SendVerificationEmailAsync(user, verificationToken, cancellationToken);

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

    public async Task<bool> VerifyEmailAsync(string verificationToken, CancellationToken cancellationToken = default)
    {

        var storedToken = await _verificationTokenRepository.GetAsync(verificationToken, cancellationToken);

        if (storedToken is null || storedToken.ExpiresAt < DateTime.Now || storedToken.User.EmailVerified)
        {
            return false;
        }

        storedToken.User.SetEmailVerified(true);

        _verificationTokenRepository.Delete(storedToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
