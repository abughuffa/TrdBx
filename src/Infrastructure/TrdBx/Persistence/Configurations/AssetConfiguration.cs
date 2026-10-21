using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class TrackedAssetConfiguration : IEntityTypeConfiguration<TrackedAsset>
{
    public void Configure(EntityTypeBuilder<TrackedAsset> builder)
    {
        builder.ToTable("TrackedAssets");

        builder.Ignore(e => e.DomainEvents);

        builder.HasKey(e => e.Id);
     
        builder.Property(e => e.Id)
        .UseIdentityColumn();

            builder.Property(e => e.TrackedAssetNo)
            .HasMaxLength(50)
            .HasColumnName("TrackedAssetNo");

            builder.Property(e => e.TrackedAssetCode)
            .HasMaxLength(50)
            .HasColumnName("TrackedAssetCode");

            builder.Property(e => e.VinSerNo)
            .HasMaxLength(50)
            .HasColumnName("VinSerNo");

            builder.Property(e => e.PlateNo)
            .HasMaxLength(50)
            .HasColumnName("PlateNo");

            builder.Property(e => e.TrackedAssetDesc)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("TrackedAssetDesc");

            builder.Property(e => e.IsAvailable)
            .IsRequired()
            .HasColumnName("IsAvailable");

            builder.Property(e => e.OldId)
            .HasColumnName("OldId");

            builder.Property(e => e.OldVehicleNo)
            .HasMaxLength(50)
            .HasColumnName("OldVehicleNo");
            


        // Indexes
        
            builder.HasIndex(e => e.TrackedAssetNo)
                .HasDatabaseName("IX_TrackedAsset_TrackedAssetNo");

            builder.HasIndex(e => e.TrackedAssetCode)
                .HasDatabaseName("IX_TrackedAsset_TrackedAssetCode");               
            
            builder.HasIndex(e => e.PlateNo)
                .HasDatabaseName("IX_TrackedAsset_PlateNo");

            builder.HasIndex(e => e.VinSerNo)
                .HasDatabaseName("IX_TrackedAsset_VinSerNo");
            

            // Relationships

            builder.HasMany(e => e.TrackingUnits)
                .WithOne(e => e.TrackedAsset)
                .HasForeignKey(e => e.TrackedAssetId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}


