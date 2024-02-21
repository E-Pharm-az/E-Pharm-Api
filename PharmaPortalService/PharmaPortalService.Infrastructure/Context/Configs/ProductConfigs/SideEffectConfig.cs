using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Configs.ProductConfigs;

public class SideEffectConfig : IEntityTypeConfiguration<SideEffect>
{
    public void Configure(EntityTypeBuilder<SideEffect> builder)
    {
        builder.Property(se => se.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(se => se.Description)
            .HasMaxLength(500);

        builder.Property(se => se.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}