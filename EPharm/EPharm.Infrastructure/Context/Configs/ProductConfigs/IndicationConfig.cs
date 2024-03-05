using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class IndicationConfig : IEntityTypeConfiguration<Indication>
{
    public void Configure(EntityTypeBuilder<Indication> builder)
    {
        builder.Property(i => i.IndicationsName)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasOne(a => a.PharmaCompany)
            .WithMany(a => a.Indications)
            .HasForeignKey(a => a.PharmaCompanyId);

        builder.Property(i => i.IndicationsDescription)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.HasMany(i => i.Products)
            .WithOne(si => si.Indication)
            .HasForeignKey(si => si.IndicationId);

        builder.Property(i => i.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
