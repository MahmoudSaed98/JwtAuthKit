namespace Application.DTOs.Requests;

public record RegisterUserRequest(string Username, string Password, string Email);
