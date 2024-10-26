using Application.DTOs.Requests;
using Bogus;
using Domain.Entities;

namespace Presentation.Services;

public static class EmployeeDummyDataGenerator
{
    public static IEnumerable<CreateEmployeeRequest> GenerateMany(int count)
    {
        var faker = new Faker<CreateEmployeeRequest>();

        faker.RuleFor(e => e.Email, f => f.Internet.Email());

        faker.RuleFor(e => e.FirstName, f => f.Name.FirstName(f.PickRandom<Bogus.DataSets.Name.Gender>()));

        faker.RuleFor(e => e.LastName, f => f.Name.LastName(f.PickRandom<Bogus.DataSets.Name.Gender>()));

        faker.RuleFor(e => e.Salary, f => decimal.Round(f.Random.Decimal(500, 15000), 2));

        return faker.Generate(count);
    }

    public static Employee GenerateOne()
    {
        var faker = new Faker<Employee>();

        faker.RuleFor(e => e.Email, f => f.Internet.Email());

        faker.RuleFor(e => e.FirstName, f => f.Name.FirstName(f.PickRandom<Bogus.DataSets.Name.Gender>()));

        faker.RuleFor(e => e.LastName, f => f.Name.LastName(f.PickRandom<Bogus.DataSets.Name.Gender>()));

        faker.RuleFor(e => e.Salary, f => decimal.Round(f.Random.Decimal(500, 15000), 2));

        return faker.Generate();
    }
}
