using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);


    Task<UserResponse?> GetByUsernameAsync(string username, CancellationToken cancellationToken
        = default);

    Task<UserResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken
        = default);


    Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken
        = default);
}
