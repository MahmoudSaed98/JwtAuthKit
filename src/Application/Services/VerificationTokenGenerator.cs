using Application.Interfaces;

namespace Application.Services;

internal class VerificationTokenGenerator : IVerificationTokenGenerator
{
    public string GenerateToken() => Guid.NewGuid().ToString("N").Substring(0, 8);
}