using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Username)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(user => user.Email)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(user => user.PasswordHash).IsRequired();

        builder.HasIndex(user => user.Username).IsUnique();
        builder.HasIndex(user => user.Email).IsUnique();

        builder.ToTable("Users");
    }
}