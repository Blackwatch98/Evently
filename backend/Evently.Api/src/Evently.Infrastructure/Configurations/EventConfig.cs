using Evently.Domain.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Infrastructure.Configurations;

public sealed class EventConfig : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events", schema: "Main");

        builder.HasKey(e => e.IdEvent);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.Title);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(e => e.ScheduledAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.Capacity)
            .IsRequired();

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Event_Capacity_Positive", "[Capacity] >= 0");
        });
    }
}
