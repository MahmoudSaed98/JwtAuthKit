using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("Api/[Controller]")]
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
        try
        {
            var response = await _authenticationService.LoginAsync(request, cancellationToken);

            return Ok(response);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }

    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var response = await _authenticationService.RefreshTokenAsync(request);

            return Ok(response);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

}