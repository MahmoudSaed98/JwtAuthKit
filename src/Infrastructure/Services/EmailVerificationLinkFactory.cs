using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Infrastructure.Services;
internal sealed class EmailVerificationLinkFactory(IHttpContextAccessor httpContextAccessor
    , LinkGenerator linkGenerator) : IEmailVerificationLinkFactory
{

    public string Create(EmailVerificationToken emailVerificationToken)
    {
        string? verificationLink = linkGenerator.GetUriByName(httpContextAccessor.HttpContext!,
            "verify-email",
            new { Token = emailVerificationToken.Token });

        return verificationLink ?? throw new InvalidOperationException("could not generate verification link.");
    }
}