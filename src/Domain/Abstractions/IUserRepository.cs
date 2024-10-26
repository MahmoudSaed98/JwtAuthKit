using Domain.Entities;

namespace Domain.Abstractions;

public interface IUserRepository : IRepository<int, User>
{
    Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string? username, CancellationToken cancellationToken = default);
}
