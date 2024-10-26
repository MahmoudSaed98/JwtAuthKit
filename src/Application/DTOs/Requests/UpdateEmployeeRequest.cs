namespace Application.DTOs.Requests;
public record UpdateEmployeeRequest(int Id, string FirstName, string LastName, string Email,
decimal Salary);
