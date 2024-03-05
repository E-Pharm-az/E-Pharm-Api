using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class SideEffectConfig : IEntityTypeConfiguration<SideEffect>
{
    public void Configure(EntityTypeBuilder<SideEffect> builder)
    {
        builder.Property(se => se.Name)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasOne(a => a.PharmaCompany)
            .WithMany(a => a.SideEffects)
            .HasForeignKey(a => a.PharmaCompanyId);

        builder.Property(se => se.Description)
            .HasMaxLength(500);
        
        builder.HasMany(se => se.Products)
            .WithOne(se => se.SideEffect)
            .HasForeignKey(se => se.SideEffectId);

        builder.Property(se => se.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
