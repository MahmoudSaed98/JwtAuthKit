using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(role => role.Id);

        builder.HasMany(role => role.Permissions)
               .WithMany(permission => permission.Roles)
               .UsingEntity<RolePermission>();

        builder.HasMany(role => role.Users)
               .WithMany(user => user.Roles)
               .UsingEntity<UserRole>();

        builder.HasData(Role.GetValues());

        builder.ToTable("Roles");
    }
}