using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Configs.ProductConfigs;

public class AllergyConfig : IEntityTypeConfiguration<Allergy>
{
    public void Configure(EntityTypeBuilder<Allergy> builder)
    {
        builder.Property(a => a.Description)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasMany(a => a.Products)
            .WithOne(a => a.Allergy)
            .HasForeignKey(a => a.AllergyId);

        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
