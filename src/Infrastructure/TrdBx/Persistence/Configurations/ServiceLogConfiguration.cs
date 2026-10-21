// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable

public class ServiceLogConfiguration : IEntityTypeConfiguration<ServiceLog>
{
    public void Configure(EntityTypeBuilder<ServiceLog> builder)
    {
        builder.ToTable("ServiceLogs");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();
        
        builder.Property(e => e.ServiceNo)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("ServiceNo");
        
        builder.Property(e => e.ServiceTask)
            .IsRequired()
            // .HasConversion<string>()
            .HasColumnName("ServiceTask");
        
        builder.Property(e => e.CustomerId)
            .IsRequired()
            .HasColumnName("CustomerId");
        
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("Description");
        
        builder.Property(e => e.SerDate)
            .IsRequired()
            .HasColumnName("SerDate");
        
        builder.Property(e => e.IsDeserved)
            .IsRequired()
            .HasColumnName("IsDeserved");
        
        builder.Property(e => e.IsBilled)
            .IsRequired()
            .HasColumnName("IsBilled");
        
        builder.Property(e => e.Amount)
            .IsRequired()
            .HasPrecision(7, 3)
            .HasColumnName("Amount");
        
        // Indexes
        builder.HasIndex(e => new { e.CustomerId, e.SerDate })
            .HasDatabaseName("IX_ServiceLog_CustomerId_SerDate");
        
        builder.HasIndex(e => e.ServiceNo)
            .HasDatabaseName("IX_ServiceLog_ServiceNo");
        
        builder.HasIndex(e => e.ServiceTask)
            .HasDatabaseName("IX_ServiceLog_ServiceTask");
        
        // Relationships
        builder.HasOne(e => e.Customer)
            .WithMany(e => e.ServiceLogs)
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.InvoiceItemGroup)
            .WithOne(e => e.ServiceLog)
            .HasForeignKey<InvoiceItemGroup>(e => e.ServiceLogId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(e => e.Subscriptions)
            .WithOne(e => e.ServiceLog)
            .HasForeignKey(e => e.ServiceLogId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(e => e.WialonTasks)
            .WithOne(e => e.ServiceLog)
            .HasForeignKey(e => e.ServiceLogId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.CreatedByUser)
            .WithMany()
            .HasForeignKey(e => e.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(e => e.CreatedByUser).AutoInclude();
    }
}


