using Evently.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", schema: "Main");

        builder.HasKey(x => x.IdUser);

        builder.Property(x => x.IdUser)
            .ValueGeneratedNever();

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Age)
            .IsRequired();

        builder.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(256);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.IsBlocked)
            .IsRequired();

        builder.Property(x => x.RoleId)
            .IsRequired();

        builder.Property(x => x.RefreshToken)
            .HasMaxLength(500);

        builder.Property(x => x.RefreshTokenExpiresAtUtc)
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .HasPrincipalKey(x => x.IdRole)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
