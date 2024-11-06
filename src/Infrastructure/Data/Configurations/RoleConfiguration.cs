using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(role => role.Id);

        builder.Property(role => role.Permissions)
               .HasConversion<int>();

        builder.HasMany(role => role.Users)
               .WithMany(user => user.Roles)
               .UsingEntity<UserRole>();


        builder.ToTable("Roles");
    }
}