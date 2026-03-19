using Evently.Infrastructure.Projections.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Infrastructure.Configurations;

public sealed class EventReadModelConfig : IEntityTypeConfiguration<EventReadModel>
{
    public void Configure(EntityTypeBuilder<EventReadModel> builder)
    {
        builder.ToTable("EntityReadModels", schema: "Main");

        builder.HasKey(r => r.IdReadModel);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(200);
    }
}
