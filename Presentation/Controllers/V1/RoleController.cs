using Application.DTOs.Requests;
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.V1;

[ApiController]
[Route("Api/Roles")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HasPermission(Permissions.CanAdd)]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var response = await _roleService.CreateRoleAsync(request, cancellationToken);

        return Ok(response);
    }

    [HasPermission(Permissions.CanRead)]
    [HttpGet("/get-role-by-name/{name}")]
    public async Task<IActionResult> GetRoleById(string name, CancellationToken cancellationToken)
    {
        var response = await _roleService.GetAsync(name);

        return response is not null ? Ok(response) : NotFound("role not found");
    }


    [HasPermission(Permissions.CanRead)]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _roleService.GetAllAsync(cancellationToken);

        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return NotFound(response.Message);
    }

    [HasPermission(Permissions.CanUpdate)]
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleToUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _roleService.AssignRoleToUserAsync(request, cancellationToken);

        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.Message);
    }

    [HasPermission(Permissions.CanUpdate)]
    [HttpPost("remove-role-from-user")]
    public async Task<IActionResult> RemoveRoleFomrUser([FromBody] RemoveRoleRequest request, CancellationToken cancellationToken)
    {
        var response = await _roleService.RemoveRoleFromUserAsync(request, cancellationToken);

        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.Message);
    }

    [HasPermission(Permissions.CanUpdate)]
    [HttpPost("assign-role-to-user")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _roleService.AssignRoleToUserAsync(request, cancellationToken);

        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.Message);
    }

}
