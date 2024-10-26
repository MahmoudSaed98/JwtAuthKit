using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(token => token.Id);

            builder.Property(token => token.IssuedAt).IsRequired();

            builder.Property(token => token.ExpiresAt).IsRequired();

            builder.Property(token => token.IsRevoked).IsRequired();

            builder.Property(token => token.RevokedAt).IsRequired(false);

            builder.HasOne<User>()
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(token => token.UserId);

            builder.ToTable("RefreshTokens");
        }
    }
}