using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne(p => p.PharmaCompany)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.PharmaCompanyId)
            .IsRequired();

        builder.HasOne(p => p.Warehouse)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.WarehouseId)
            .IsRequired();
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.StrengthMg)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.MaxDayFrequency)
            .IsRequired();

        builder.Property(p => p.MaxSupplyDaysPeriod)
            .IsRequired();

        builder.Property(p => p.ContraindicationsDescription)
            .HasMaxLength(500);

        builder.Property(p => p.StorageConditionDescription)
            .HasMaxLength(500);

        builder.HasOne(p => p.SpecialRequirement)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.SpecialRequirementsId);
        
        builder.HasOne(p => p.Manufacturer)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.ManufacturerId);

        builder.Property(p => p.ExpiryDate)
            .HasColumnType("date");
        
        builder.Property(p => p.Stock)
            .IsRequired();

        builder.Property(p => p.Price)
            .IsRequired();

        builder.Property(p => p.BatchNumber)
            .HasMaxLength(50);

        builder.Property(p => p.Barcode)
            .HasMaxLength(50);

        builder.HasMany(p => p.ActiveIngredients)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);
        
        builder.HasMany(p => p.DosageForms)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);

        builder.HasMany(p => p.RouteOfAdministrations)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);

        builder.HasMany(p => p.SideEffects)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);

        builder.HasMany(p => p.UsageWarnings)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);

        builder.HasMany(p => p.Allergies)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);

        builder.HasMany(p => p.Indications)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);

        builder.Property(p => p.PackagingWeight)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
