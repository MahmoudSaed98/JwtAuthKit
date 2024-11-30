using Application.Interfaces;

namespace Application.Services;

internal class EmailConfirmationTokenProvider : IEmailConfirmationTokenProvider
{
    // this implementation just for demo .
    public string GenerateToken() => Guid.NewGuid().ToString("N").Substring(0, 8);
}