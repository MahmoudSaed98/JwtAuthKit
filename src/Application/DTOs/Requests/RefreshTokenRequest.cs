namespace Application.DTOs.Requests;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);
