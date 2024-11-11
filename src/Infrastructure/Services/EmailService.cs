using Application.Interfaces;
using FluentEmail.Core;

namespace Infrastructure.Services;

internal sealed class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string message, CancellationToken cancellationToken = default)
    {
        var response = await fluentEmail
                             .To(toEmail)
                             .Subject(subject)
                             .Body(message, isHtml: true)
                             .SendAsync(cancellationToken);
    }
}
