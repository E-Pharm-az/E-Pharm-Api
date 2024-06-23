using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.ProductConfigs;

public class WarehouseConfig : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(w => w.Address)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasOne(w => w.Pharmacy)
            .WithMany(p => p.Warehouses)
            .HasForeignKey(w => w.PharmaCompanyId)
            .IsRequired();
        
        builder.Property(w => w.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
