using EPharm.Infrastructure.Context.Entities.Junctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.JunctionConfigs;

public class WarehouseProductConfig : IEntityTypeConfiguration<WarehouseProduct>
{
    public void Configure(EntityTypeBuilder<WarehouseProduct> builder)
    {
        builder.HasOne(wp => wp.Warehouse)
            .WithMany(w => w.WarehouseProducts)
            .HasForeignKey(wp => wp.WarehouseId)
            .IsRequired();

        builder.HasOne(wp => wp.Product)
            .WithMany(p => p.WarehouseProducts)
            .HasForeignKey(wp => wp.ProductId)
            .IsRequired();

        builder.Property(wp => wp.Quantity)
            .IsRequired();

        builder.Property(wp => wp.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
