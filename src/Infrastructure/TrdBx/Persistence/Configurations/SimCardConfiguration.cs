using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
// public class SimCardConfiguration : IEntityTypeConfiguration<SimCard>
// {
//     public void Configure(EntityTypeBuilder<SimCard> builder)
//     {
//         builder.HasIndex(t => t.SimCardNo).IsUnique(true);
//         builder.Property(t => t.SimCardNo).HasMaxLength(50).IsRequired();
        
//     }
// }

public class SimCardConfiguration : IEntityTypeConfiguration<SimCard>
{
    public void Configure(EntityTypeBuilder<SimCard> builder)
    {
        builder.ToTable("SimCards");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();
        
        builder.Property(e => e.SimCardNo)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("SimCardNo");
        
        builder.Property(e => e.ICCID)
            .HasMaxLength(50)
            .HasColumnName("ICCID");
        
        builder.Property(e => e.SPackageId)
            .IsRequired()
            .HasColumnName("SPackageId");
        
        builder.Property(e => e.SStatus)
            .IsRequired()
            // .HasConversion<string>()
            .HasColumnName("SStatus");
        
        builder.Property(e => e.IsOwned)
            .IsRequired()
            .HasColumnName("IsOwned");
        
        builder.Property(e => e.ExDate)
            .HasColumnName("ExDate");
        
        builder.Property(e => e.OldId)
            .HasColumnName("OldId");
        
        // Indexes
        builder.HasIndex(e => e.SimCardNo)
            // .IsUnique()
            .HasDatabaseName("IX_SimCard_SimCardNo");
        
        builder.HasIndex(e => e.ICCID)
            .HasDatabaseName("IX_SimCard_ICCID");
        
        builder.HasIndex(e => e.SPackageId)
            .HasDatabaseName("IX_SimCard_SPackageId");
        
        // Relationships
        builder.HasOne(e => e.SPackage)
            .WithMany(e => e.SimCards)
            .HasForeignKey(e => e.SPackageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}