using Domain.Entities;

namespace Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message, CancellationToken cancellationToken = default);

    Task SendVerificationEmailAsync(User user, EmailVerificationToken token, CancellationToken cancellationToken = default);
}