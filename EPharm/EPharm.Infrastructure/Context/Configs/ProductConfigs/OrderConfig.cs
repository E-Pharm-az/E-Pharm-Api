using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.TrackingId)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(o => o.OrderStatus)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(o => o.TotalPrice)
            .IsRequired();
        
        builder.Property(o => o.ShippingAddress)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasOne(o => o.PharmaCompany)
            .WithMany(pc => pc.Orders)
            .HasForeignKey(o => o.PharmaCompanyId)
            .IsRequired();
        
        builder.HasOne(o => o.Warehouse)
            .WithMany(w => w.Orders)
            .HasForeignKey(o => o.WarehouseId)
            .IsRequired();
        
        builder.Property(o => o.CreatedAt)
            .IsRequired();
    }
}
