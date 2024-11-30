namespace Application.Interfaces;

public interface IEmailConfirmationTokenProvider
{
    string GenerateToken();
}
