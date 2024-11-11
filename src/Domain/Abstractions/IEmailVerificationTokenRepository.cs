using Domain.Entities;

namespace Domain.Abstractions;

public interface IEmailVerificationTokenRepository :
    IRepository<int, EmailVerificationToken>
{
    Task<EmailVerificationToken?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<EmailVerificationToken?> GetAsync(string token, CancellationToken cancellationToken = default);
}
