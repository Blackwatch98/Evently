using Evently.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Infrastructure.Configurations
{
    public sealed class OutboxMessageConfig : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages", schema: "EMain");

            builder.HasKey(e => e.IdOutboxMessage);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
