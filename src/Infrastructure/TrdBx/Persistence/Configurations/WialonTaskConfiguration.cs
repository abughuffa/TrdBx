//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class WialonTaskConfiguration : IEntityTypeConfiguration<WialonTask>
{
    public void Configure(EntityTypeBuilder<WialonTask> builder)
    {
        

        builder.ToTable("WialonTasks");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();
        
        builder.Property(e => e.ServiceLogId)
            .IsRequired()
            .HasColumnName("ServiceLogId");
        
        builder.Property(e => e.TrackingUnitId)
            .IsRequired()
            .HasColumnName("TrackingUnitId");
        
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("Description");
        
        builder.Property(e => e.WialonAPIAction)
            // .HasConversion<string>()
            .HasColumnName("WialonAPIAction");
        
        builder.Property(e => e.ExcDate)
            .IsRequired()
            .HasColumnName("ExcDate");
        
        builder.Property(e => e.IsExecuted)
            .IsRequired()
            .HasColumnName("IsExecuted");
        
        // Indexes
        builder.HasIndex(e => new { e.TrackingUnitId, e.IsExecuted })
            .HasDatabaseName("IX_WialonTask_TrackingUnitId_IsExecuted");
        
        builder.HasIndex(e => e.ServiceLogId)
            .HasDatabaseName("IX_WialonTask_ServiceLogId");
        
        // Relationships
        builder.HasOne(e => e.ServiceLog)
            .WithMany(e => e.WialonTasks)
            .HasForeignKey(e => e.ServiceLogId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.TrackingUnit)
            .WithMany(e => e.WialonTasks)
            .HasForeignKey(e => e.TrackingUnitId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}



