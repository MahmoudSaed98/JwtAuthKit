namespace Application.DTOs.Requests;

public record GetEmployeesRequest(int Page, int PageSize,
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder);