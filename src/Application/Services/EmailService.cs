using Application.Common;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services;

internal class EmailService : IEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly ILinkService _linkGenerator;
    private readonly IVerificationTokenGenerator _verificationTokenGenerator;
    private readonly IEmailContentGenerator _emailContentGenerator;

    public EmailService(IEmailSender emailSender,
        ILinkService linkGenerator,
        IVerificationTokenGenerator verificationTokenGenerator,
        IEmailContentGenerator emailContentGenerator)
    {
        _emailSender = emailSender;
        _linkGenerator = linkGenerator;
        _verificationTokenGenerator = verificationTokenGenerator;
        _emailContentGenerator = emailContentGenerator;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message, CancellationToken cancellationToken = default)
    {
        await _emailSender.SendEmailAsync(toEmail, subject, message, cancellationToken);
    }

    public async Task SendVerificationEmailAsync(User user, EmailVerificationToken token, CancellationToken cancellationToken = default)
    {
        string verificationLink = _linkGenerator.Generate(EndpointNames.VerifyEmail, routeValue: new { Token = token.Token });

        string content = _emailContentGenerator.GenerateVerificationEmailContent(user.Username, verificationLink, token.ExpiresAt);

        await _emailSender.SendEmailAsync(user.Email, EmailSubjects.EmailVerification, content, cancellationToken);
    }
}