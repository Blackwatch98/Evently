using Evently.Domain.RegistrationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Infrastructure.Configurations;

public sealed class RegistrationConfig : IEntityTypeConfiguration<Registration>
{
    public void Configure(EntityTypeBuilder<Registration> builder)
    {
        builder.ToTable("Registrations", schema: "Main");

        builder.HasKey(r => r.IdRegistration);

        builder.Property(r => r.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(r => new { r.EventId, r.Email })
            .IsUnique();

        builder.Property(r => r.RegisteredAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);
    }
}
