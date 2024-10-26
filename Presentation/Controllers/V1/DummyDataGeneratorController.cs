using Domain.Entities;
using Infrastructure.Data.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Presentation.Services;

namespace Presentation.Controllers.V1;

[ApiController]
[Route("Api/[Controller]")]
public class DummyDataGeneratorController : ControllerBase
{
    private ApplicationDbContext _context;

    public DummyDataGeneratorController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("generate-many")]
    public async Task<IActionResult> GenerateMany(int count)
    {
        var fakeEmployeeData = EmployeeDummyDataGenerator.GenerateMany(count);

        var convertedFromDto = fakeEmployeeData.Select(e => Employee.Create(e.FirstName,
            e.LastName, e.Email, e.Salary));

        await _context.Employees.AddRangeAsync(convertedFromDto);

        await _context.SaveChangesAsync();

        return Ok(fakeEmployeeData);
    }


}
