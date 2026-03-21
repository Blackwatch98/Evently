using Evently.Domain.Aggregates.RegistrationAggregate;
using Evently.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Infrastructure.Configurations;

internal sealed class RegistrationConfig : IEntityTypeConfiguration<Registration>
{
    public void Configure(EntityTypeBuilder<Registration> builder)
    {
        builder.ToTable("Registrations", schema: "Main");

        builder.HasKey(x => x.IdRegistration);

        builder.Property(x => x.IdRegistration)
            .ValueGeneratedNever();

        builder.Property(x => x.EventId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.RegisteredAt)
            .IsRequired();

        builder.HasIndex(x => new { x.EventId, x.UserId })
            .IsUnique();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.IdUser)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
