using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs.PharmaConfigs;

public class PharmaCompanyManagerConfig : IEntityTypeConfiguration<PharmaCompanyManager>
{
    public void Configure(EntityTypeBuilder<PharmaCompanyManager> builder)
    {
        builder.Property(pcm => pcm.ExternalId)
            .IsRequired();
        
        builder.Property(pcm => pcm.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pcm => pcm.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pcm => pcm.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pcm => pcm.PhoneNumber)
            .HasMaxLength(20);

        builder.HasOne(pcm => pcm.PharmaCompany)
            .WithMany(pc => pc.Managers)
            .HasForeignKey(pcm => pcm.PharmaCompanyId);

        builder.Property(pcm => pcm.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}