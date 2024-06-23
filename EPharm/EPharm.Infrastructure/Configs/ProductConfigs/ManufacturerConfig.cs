using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class ManufacturerConfig : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> builder)
    {
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasOne(a => a.Pharmacy)
            .WithMany(a => a.Manufacturers)
            .HasForeignKey(a => a.PharmaCompanyId);

        builder.Property(m => m.Country)
            .HasMaxLength(255);
        
        builder.Property(m => m.Website)
            .HasMaxLength(255);
        
        builder.Property(m => m.Email)
            .HasMaxLength(255);

        builder.Property(m => m.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
