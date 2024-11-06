namespace Application.DTOs.Responses;

public record UserResponse(int Id, string Username, string Email, List<string> Roles);
