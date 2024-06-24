using EPharm.Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.ProductConfigs;

public class SideEffectConfig : IEntityTypeConfiguration<SideEffect>
{
    public void Configure(EntityTypeBuilder<SideEffect> builder)
    {
        builder.Property(se => se.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(se => se.Description)
            .HasMaxLength(500);
        
        builder.HasMany(se => se.Products)
            .WithOne(se => se.SideEffect)
            .HasForeignKey(se => se.SideEffectId);

        builder.Property(se => se.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
