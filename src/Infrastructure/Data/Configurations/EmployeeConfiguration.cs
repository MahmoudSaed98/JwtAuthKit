using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.Salary)
               .HasPrecision(8, 3)
               .IsRequired();

        builder.HasIndex(e => new { e.FirstName, e.LastName });

        builder.HasIndex(e => e.Email).IsUnique();

        builder.HasIndex(e => e.Salary);
    }
}
