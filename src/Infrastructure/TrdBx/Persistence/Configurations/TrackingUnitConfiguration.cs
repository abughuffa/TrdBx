// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
// public class TrackingUnitConfiguration : IEntityTypeConfiguration<TrackingUnit>
// {
//     public void Configure(EntityTypeBuilder<TrackingUnit> builder)
//     {
//         //builder.Property(t => t.SNo).IsUnique(true);
//         builder.Property(t => t.SNo).HasMaxLength(50).IsRequired();
//         builder.Ignore(e => e.DomainEvents);
//     }
// }

public class TrackingUnitConfiguration : IEntityTypeConfiguration<TrackingUnit>
{
    public void Configure(EntityTypeBuilder<TrackingUnit> builder)
    {
        builder.ToTable("TrackingUnits");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();
        
        builder.Property(e => e.SNo)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("SNo");
        
        builder.Property(e => e.Imei)
            .HasMaxLength(50)
            .HasColumnName("Imei");
        
        builder.Property(e => e.UnitName)
            .HasMaxLength(100)
            .HasColumnName("UnitName");
        
        builder.Property(e => e.TrackingUnitModelId)
            .IsRequired()
            .HasColumnName("TrackingUnitModelId");
        
        builder.Property(e => e.UStatus)
            .IsRequired()
            // .HasConversion<string>()
            .HasColumnName("UStatus");
        
        builder.Property(e => e.InsMode)
            .IsRequired()
            // .HasConversion<string>()
            .HasColumnName("InsMode");
        
        builder.Property(e => e.WryDate)
            .HasColumnName("WryDate");
        
        builder.Property(e => e.TrackedAssetId)
            .HasColumnName("TrackedAssetId");
        
        builder.Property(e => e.SimCardId)
            .HasColumnName("SimCardId");
        
        builder.Property(e => e.CustomerId)
            .HasColumnName("CustomerId");
        
        builder.Property(e => e.IsOnWialon)
            .IsRequired()
            .HasColumnName("IsOnWialon");
        
        builder.Property(e => e.WStatus)
            // .HasConversion<string>()
            .HasColumnName("WStatus");
        
        builder.Property(e => e.WUnitId)
            .HasColumnName("WUnitId");
        
        builder.Property(e => e.OldId)
            .HasColumnName("OldId");
        
        // Indexes
        builder.HasIndex(e => e.SNo)
            // .IsUnique()
            .HasDatabaseName("IX_TrackingUnit_SNo");
        
        builder.HasIndex(e => e.Imei)
            // .IsUnique()
            .HasDatabaseName("IX_TrackingUnit_Imei")
            .HasFilter("\"Imei\" IS NOT NULL");
        
        builder.HasIndex(e => new { e.CustomerId, e.UStatus })
            .HasDatabaseName("IX_TrackingUnit_CustomerId_UStatus");
        
        builder.HasIndex(e => new { e.UStatus, e.IsOnWialon })
            .HasDatabaseName("IX_TrackingUnit_UStatus_IsOnWialon");
        
        builder.HasIndex(e => e.TrackingUnitModelId)
            .HasDatabaseName("IX_TrackingUnit_TrackingUnitModelId");
        
        // Relationships
        builder.HasOne(e => e.TrackingUnitModel)
            .WithMany(e => e.TrackingUnits)
            .HasForeignKey(e => e.TrackingUnitModelId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.SimCard)
            .WithOne(e => e.TrackingUnit)
            .HasForeignKey<TrackingUnit>(e => e.SimCardId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(e => e.Customer)
            .WithMany(e => e.TrackingUnits)
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(e => e.TrackedAsset)
            .WithMany(e => e.TrackingUnits)
            .HasForeignKey(e => e.TrackedAssetId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(e => e.Subscriptions)
            .WithOne(e => e.TrackingUnit)
            .HasForeignKey(e => e.TrackingUnitId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(e => e.WialonTasks)
            .WithOne(e => e.TrackingUnit)
            .HasForeignKey(e => e.TrackingUnitId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(e => e.Tickets)
            .WithOne(e => e.TrackingUnit)
            .HasForeignKey(e => e.TrackingUnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


