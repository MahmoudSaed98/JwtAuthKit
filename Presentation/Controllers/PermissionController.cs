using Application.DTOs.Requests;
using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("Api/Permissions")]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HasPermission(Permissions.CanUpdate)]
    [HttpPut("grant-permission-to-role")]
    public async Task<IActionResult> AssignPermissionToRole([FromBody] GrantPermissionRequest request, CancellationToken cancellationToken)
    {
        var response = await _permissionService.GrantPermissionToRoleAsync(request, cancellationToken);

        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.Message);
    }

    [HasPermission(Permissions.CanUpdate)]
    [HttpPut("revoke-permission-from-role")]
    public async Task<IActionResult> RevokePermissionFromRole(int permissionId, int roleId, CancellationToken cancellationToken)
    {
        var response = await _permissionService.RevokePermissionFromRoleAsync(permissionId, roleId, cancellationToken);

        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        return BadRequest(response.Message);
    }

    [HasPermission(Permissions.CanRead)]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _permissionService.GetAllPermissionsAsync(cancellationToken);

        return response is not null ? Ok(response) : NotFound();
    }

}
