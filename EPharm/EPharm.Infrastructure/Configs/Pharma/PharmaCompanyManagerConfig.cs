using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Configs.Pharma;

public class PharmaCompanyManagerConfig : IEntityTypeConfiguration<PharmacyStaff>
{
    public void Configure(EntityTypeBuilder<PharmacyStaff> builder)
    {
        builder.Property(pcm => pcm.ExternalId)
            .IsRequired();

        builder.Property(pcm => pcm.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasOne(pcm => pcm.Pharmacy)
            .WithMany(pc => pc.Managers)
            .HasForeignKey(pcm => pcm.PharmaCompanyId);

        builder.Property(pcm => pcm.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}