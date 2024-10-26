namespace Application.DTOs.Requests;
public record CreateEmployeeRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public decimal Salary { get; set; }


}
