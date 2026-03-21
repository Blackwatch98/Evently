using Evently.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Infrastructure.Configurations;

internal sealed class OutboxMessageConfig : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages", schema: "Main");

        builder.HasKey(e => e.IdOutboxMessage);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(200);
    }
}
