using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

internal sealed class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.HasKey(token => token.Id);

        builder.HasOne(token => token.User)
               .WithMany()
               .HasForeignKey(token => token.UserId);

        builder.Property(token => token.Token).IsRequired();

        builder.Property(token => token.CreatedAt).IsRequired();

        builder.Property(token => token.ExpiresAt).IsRequired();

        builder.HasIndex(token => token.UserId);

        builder.HasIndex(token => token.Token).IsUnique();

        builder.ToTable("EmailVerificationTokens");
    }
}
