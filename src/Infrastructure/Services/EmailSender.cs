using Domain.Abstractions;
using FluentEmail.Core;

namespace Infrastructure.Services;

internal sealed class EmailSender(IFluentEmail fluentEmail) : IEmailSender
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
