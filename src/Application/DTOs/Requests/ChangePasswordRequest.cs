namespace Application.DTOs.Requests;

public record ChangePasswordRequest(string Email, string CurrentPassword, string NewPassword);
