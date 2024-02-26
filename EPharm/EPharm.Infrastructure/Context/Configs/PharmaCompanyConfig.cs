using EPharm.Infrastructure.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPharm.Infrastructure.Context.Configs;

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
        
        builder.HasOne(pc => pc.PharmaCompanyOwner)
            .WithOne(pc => pc.PharmaCompany)
            .HasForeignKey<PharmaCompany>(a => a.PharmaCompanyOwnerId);

        builder.HasOne(pc => pc.Address)
            .WithOne(pc => pc.PharmaCompany)
            .HasForeignKey<Address>(a => a.PharmaCompanyId);

        builder.Property(pc => pc.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
