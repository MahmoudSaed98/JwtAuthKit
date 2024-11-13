namespace Application.Interfaces;
public interface IEmailContentGenerator
{
    string GenerateVerificationEmailContent(string username, string verificationLink, DateTime tokenValidity);
}
