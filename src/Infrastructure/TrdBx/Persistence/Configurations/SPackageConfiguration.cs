using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
// public class SPackageConfiguration : IEntityTypeConfiguration<SPackage>
// {
//     public void Configure(EntityTypeBuilder<SPackage> builder)
//     {
//         builder.HasIndex(t => t.Name).IsUnique(true);
//         builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
//         builder.Ignore(e => e.DomainEvents);
//     }
// }

public class SPackageConfiguration : IEntityTypeConfiguration<SPackage>
{
    public void Configure(EntityTypeBuilder<SPackage> builder)
    {
        builder.ToTable("SPackages");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();

        builder.Property(e => e.SProviderId)
            .IsRequired()
            .HasColumnName("SProviderId");
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("Name");

        // Indexes
        builder.HasIndex(e => e.Name)
            .HasDatabaseName("IX_SPackage_Name");
        
        // Relationships
        builder.HasMany(e => e.SimCards)
            .WithOne(e => e.SPackage)
            .HasForeignKey(e => e.SPackageId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}


