using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities.ProductEntities;

namespace PharmaPortalService.Infrastructure.Context.Configs.ProductConfigs;

public class UsageWarningConfig : IEntityTypeConfiguration<UsageWarning>
{
    public void Configure(EntityTypeBuilder<UsageWarning> builder)
    {
        builder.Property(uw => uw.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(uw => uw.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
