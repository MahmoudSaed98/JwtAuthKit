using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class PermisionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(50);

        builder.Property(x => x.Value).IsRequired();

        //IEnumerable<Permission> permissions = Enum.GetValues<Domain.Enums.Permissions>()
        //                       .Where(p => !p.HasFlag(Domain.Enums.Permissions.None))
        //                      .Select(permissionEnum =>
        //                   new Permission((int)permissionEnum, permissionEnum.ToString()));

        //builder.HasData(permissions);

        builder.ToTable("Permissions");
    }
}
