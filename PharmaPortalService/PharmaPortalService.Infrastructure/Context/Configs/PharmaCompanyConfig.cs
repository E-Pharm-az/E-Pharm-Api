using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaPortalService.Infrastructure.Context.Entities;

namespace PharmaPortalService.Infrastructure.Context.Configs;

public class PharmaCompanyConfig : IEntityTypeConfiguration<PharmaCompany>
{
    public void Configure(EntityTypeBuilder<PharmaCompany> builder)
    {
        builder.Property(pc => pc.CompanyName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pc => pc.Location)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pc => pc.ContactEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pc => pc.ContactPhone)
            .HasMaxLength(20);

        builder.HasMany(pc => pc.PharmaCompanyManagers)
            .WithOne(pcm => pcm.PharmaCompany)
            .HasForeignKey(pcm => pcm.PharmaCompanyId);

        builder.Property(pc => pc.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
