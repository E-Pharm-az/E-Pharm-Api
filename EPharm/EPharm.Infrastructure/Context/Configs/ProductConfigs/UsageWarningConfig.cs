using EPharm.Infrastructure.Context.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.ProductConfigs;

public class UsageWarningConfig : IEntityTypeConfiguration<UsageWarning>
{
    public void Configure(EntityTypeBuilder<UsageWarning> builder)
    {
        builder.Property(uw => uw.Description)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.HasMany(uw => uw.Products)
            .WithOne(uw => uw.UsageWarning)
            .HasForeignKey(uw => uw.UsageWarningId);

        builder.Property(uw => uw.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
