using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("Api/[Controller]")]
[ApiVersion("1.0")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _authenticationService.LoginAsync(request, cancellationToken);

        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return Unauthorized(response.Message);
    }

    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _authenticationService.RegisterUserAsync(request, cancellationToken);

        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.Message);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var response = await _authenticationService.RefreshTokenAsync(request);

        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return Unauthorized(response.Message);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToke([FromBody] RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var response = await _authenticationService.RevokeRefreshTokenAsync(request, cancellationToken);

        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return NotFound(response.Message);
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail(string token)
    {
        var response = await _authenticationService.VerifyEmailAsync(token);

        return response ? Ok("email confirmed successfully") : BadRequest("invalid or expired token");
    }
}