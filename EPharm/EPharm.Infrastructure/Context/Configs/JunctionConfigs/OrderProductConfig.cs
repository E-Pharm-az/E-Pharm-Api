using EPharm.Infrastructure.Context.Entities.Junctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.JunctionConfigs;

public class OrderProductConfig : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId)
            .IsRequired();

        builder.HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId)
            .IsRequired();

        builder.Property(op => op.Quantity)
            .IsRequired();

        builder.Property(op => op.TotalPrice)
            .IsRequired();

        builder.Property(op => op.CreatedAt)
            .IsRequired();
    }
}