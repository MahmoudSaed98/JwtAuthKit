using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers.V1;

[Route("api/[Controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("get-user-by-id/{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);

        return user is null ? NotFound("user not found.") : Ok(user);
    }

    [Authorize]
    [HttpGet("get-by-email/{email}")]
    public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
    {

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("email cannot be empty.");
        }

        var user = await _userService.GetByEmailAsync(email, cancellationToken);

        return user is null ? NotFound("user not found.") : Ok(user);
    }

    [Authorize]
    [HttpGet("get-by-username/{username}")]
    public async Task<IActionResult> GetByUsername(string username, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(username))
        {
            return BadRequest("username cannot by empty.");
        }

        var user = await _userService.GetByUsernameAsync(username, cancellationToken);

        return user is null ? NotFound("user not found.") : Ok(user);
    }


    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var success = await _userService.ChangePasswordAsync(request, cancellationToken);

        return success ? Ok("password has been changed successfully.") :
                         BadRequest("Invalid Request.");
    }
}
