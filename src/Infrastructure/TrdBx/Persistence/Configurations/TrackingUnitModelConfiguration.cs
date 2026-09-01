// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable


public class TrackingUnitModelConfiguration : IEntityTypeConfiguration<TrackingUnitModel>
{
    public void Configure(EntityTypeBuilder<TrackingUnitModel> builder)
    {
        builder.ToTable("TrackingUnitModels");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();
        
        builder.Property(e => e.WialonName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("WialonName");
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("Name");
        
        builder.Property(e => e.WhwTypeId)
            .IsRequired()
            .HasColumnName("WhwTypeId");
        
        builder.Property(e => e.DefaultHost)
            .IsRequired()
            .HasPrecision(7, 3)
            .HasColumnName("DefaultHost");
        
        builder.Property(e => e.DefaultGprs)
            .IsRequired()
            .HasPrecision(7, 3)
            .HasColumnName("DefaultGprs");
        
        builder.Property(e => e.DefaultPrice)
            .IsRequired()
            .HasPrecision(7, 3)
            .HasColumnName("DefaultPrice");
        
        builder.Property(e => e.PortNo1)
            .HasColumnName("PortNo1");
        
        builder.Property(e => e.PortNo2)
            .HasColumnName("PortNo2");
        
        builder.Property(e => e.OldId)
            .HasColumnName("OldId");
        
        // Indexes
        builder.HasIndex(e => e.Name)
            .HasDatabaseName("IX_TrackingUnitModel_Name");
        
        builder.HasIndex(e => e.WialonName)
            .HasDatabaseName("IX_TrackingUnitModel_WialonName");
        
        // Relationships
        builder.HasMany(e => e.TrackingUnits)
            .WithOne(e => e.TrackingUnitModel)
            .HasForeignKey(e => e.TrackingUnitModelId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(e => e.CusPrices)
            .WithOne(e => e.TrackingUnitModel)
            .HasForeignKey(e => e.TrackingUnitModelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


