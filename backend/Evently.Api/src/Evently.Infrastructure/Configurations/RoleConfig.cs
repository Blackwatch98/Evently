using Evently.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Infrastructure.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles", schema: "Dict");

        builder.HasKey(x => x.IdRole);

        builder.Property(x => x.IdRole)
            .IsRequired()
            .ValueGeneratedNever();

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Roles_IdRole_Positive", "IdRole > 0");
            t.HasCheckConstraint("CK_Roles_Priority", "Priority > 0");
        });

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Priority)
            .IsRequired();

        builder.HasData(
            new
            {
                IdRole = 1,
                Name = "Admin",
                Description = "Administrator with full access to the system.",
                Priority = 1
            },
            new
            {
                IdRole = 2,
                Name = "User",
                Description = "Regular application user with limited access.",
                Priority = 2
            }
        );
    }
}
