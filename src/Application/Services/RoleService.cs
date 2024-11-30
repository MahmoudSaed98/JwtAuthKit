using Application.Common;
using Application.Common.Constants;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Extensions;

namespace Application.Services;

internal class RoleService : IRoleService
{
    private const byte MinimumAllowedRoles = 1;

    private readonly IRoleRepository _rolesRepository;

    private readonly IUserRepository _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IRoleRepository rolesRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository)
    {
        _rolesRepository = rolesRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;

    }

    public async Task<Result<string>> AssignRoleToUserAsync(AssignRoleToUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
        {
            return Result<string>.Failure($"the user '{request.Username}' not found.");
        }

        var roleToAssign = await _rolesRepository.GetRoleAsync(request.Role, cancellationToken);

        if (roleToAssign is null)
        {
            return Result<string>.Failure($"the role '{request.Role}' was not found.");
        }

        user.AssignRole(roleToAssign);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success($"Role '{request.Role}' assigned to user '{request.Username}' successfully.");
    }

    public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
    {
        if (await _rolesRepository.IsRoleExists(request.Name))
        {
            throw new Exception("the role is already exists.");
        }

        var role = new Role(request.Name);

        _rolesRepository.Insert(role);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RoleResponse(role.Id, role.Name);
    }

    public async Task<Result<string>> DeleteRoleAsync(DeleteRoleRequest request, CancellationToken cancellationToken = default)
    {
        var existingRole = await _rolesRepository.GetRoleAsync(request.Role, cancellationToken);

        if (existingRole is null)
        {
            return Result<string>.Failure($"role '{request.Role} not found.'");
        }

        _rolesRepository.Delete(existingRole);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success($"role has been deleted successfully.");
    }

    public async Task<Result<IEnumerable<RoleResponseWithPermissions>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _rolesRepository.GetAllAsync(cancellationToken);

        if (roles is null)
        {
            return Result<IEnumerable<RoleResponseWithPermissions>>.Failure("not roles found.");
        }

        var rolesDto = roles.Select(x => new RoleResponseWithPermissions(x.Id, x.Name,
            x.Permissions.ToStringArray()));

        return Result<IEnumerable<RoleResponseWithPermissions>>.Success(rolesDto);
    }

    public async Task<RoleResponseWithPermissions?> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        var role = await _rolesRepository.GetRoleAsync(name, cancellationToken);

        if (role is not null)
        {
            var permissions = role.Permissions.ToStringArray();

            return new RoleResponseWithPermissions(role.Id, role.Name, permissions);
        }

        return null;
    }

    public async Task<Result<string>> RemoveRoleFromUserAsync(RemoveRoleRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure($"user '{request.Username}' not found.");
        }

        if (user.Roles.Count == MinimumAllowedRoles)
        {
            return Result<string>.Failure(ErrorMessages.RoleRequired);
        }

        var roleToRemove = await _rolesRepository.GetRoleAsync(request.Role, cancellationToken);

        if (roleToRemove is null)
        {
            return Result<string>.Failure($"Role '{request.Role}' not found.");
        }

        user.RemoveRole(roleToRemove);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success($"Role '{request.Role}' removed from user '{request.Username}' successfully.");
    }

    public async Task<bool> RoleExistsAsync(string role, CancellationToken cancellationToken = default)
    {
        return await _rolesRepository.IsRoleExists(role, cancellationToken);
    }

    public async Task<Result<string>> UpdateAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default)
    {
        var roleToUpdate = await _rolesRepository.GetRoleAsync(request.RoleToUpdate, cancellationToken);

        if (roleToUpdate is null)
        {
            return Result<string>.Failure($"role {request.RoleToUpdate} not found.");
        }

        roleToUpdate.SetName(request.NewValue);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success("role updated successfully.");
    }
}
