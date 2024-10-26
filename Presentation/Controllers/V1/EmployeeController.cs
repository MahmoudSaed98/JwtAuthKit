using Application.DTOs.Requests;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.V1;

[Route("Api/[Controller]/v1")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IValidator<CreateEmployeeRequest> _validator;
    public EmployeeController(IEmployeeService employeeService, IValidator<CreateEmployeeRequest> validator) =>
        (_employeeService, _validator) = (employeeService, validator);

    [HttpPost("create-employee")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var newlyInsertedEmployee = await _employeeService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { Id = newlyInsertedEmployee.Id },
            newlyInsertedEmployee);
    }

    [HttpGet("get-by-Id/{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var employee = await _employeeService.GetByIdAsync(id, cancellationToken);

        return employee == null ? NotFound() : Ok(employee);
    }

    [HttpGet("get-by-email/{email}")]
    public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
    {
        var employee = await _employeeService.GetByEmailAsync(email, cancellationToken);

        return employee == null ? NotFound() : Ok(employee);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _employeeService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpGet("get-employees")]
    public async Task<IActionResult> GetPagedList([FromQuery] GetEmployeesRequest request, CancellationToken cancellationToken)
    {
        var result = await _employeeService.GetEmployeesPagedList(request, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("id not matche.");
        }

        await _employeeService.UpdateAsync(request, cancellationToken);

        return NoContent();
    }

}
