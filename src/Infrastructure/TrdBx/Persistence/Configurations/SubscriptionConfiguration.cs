// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable


// Configurations/SubscriptionConfiguration.cs
public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");

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
        
        builder.Property(e => e.CaseCode)
            .IsRequired()
            .HasColumnName("CaseCode");
        
        builder.Property(e => e.LastPaidFees)
            .IsRequired()
            // .HasConversion<string>()
            .HasColumnName("LastPaidFees");
        
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("Description");
        
        builder.Property(e => e.SsDate)
            .IsRequired()
            .HasColumnName("SsDate");
        
        builder.Property(e => e.SeDate)
            .IsRequired()
            .HasColumnName("SeDate");
        
        builder.Property(e => e.DailyFees)
            .IsRequired()
            .HasPrecision(7, 3)
            .HasColumnName("DailyFees");
        


        // Ignore computed properties
        builder.Ignore(e => e.Days);
        builder.Ignore(e => e.Amount);
        
        // Indexes
        builder.HasIndex(e => new { e.TrackingUnitId, e.SsDate, e.SeDate })
            .HasDatabaseName("IX_Subscription_TrackingUnitId_Dates");
        
        builder.HasIndex(e => e.ServiceLogId)
            .HasDatabaseName("IX_Subscription_ServiceLogId");
        
        // Index for active subscriptions
        // builder.HasIndex(e => e.TrackingUnitId)
        //     .HasDatabaseName("IX_Subscription_TrackingUnitId_Active")
        //     .HasFilter("\"SeDate\" >= CURRENT_DATE");
        
        // Relationships
        builder.HasOne(e => e.ServiceLog)
            .WithMany(e => e.Subscriptions)
            .HasForeignKey(e => e.ServiceLogId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.TrackingUnit)
            .WithMany(e => e.Subscriptions)
            .HasForeignKey(e => e.TrackingUnitId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.InvoiceItem)
            .WithOne(e => e.Subscription)
            .HasForeignKey<InvoiceItem>(e => e.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}







// public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
// {
//     public void Configure(EntityTypeBuilder<Subscription> builder)
//     {
//         builder.Property(t => t.TrackingUnitId).IsRequired();
//         builder.Property(t => t.Description).HasMaxLength(256).IsRequired();
//         builder.Property(t => t.SsDate).IsRequired();
//         builder.Property(t => t.SeDate).IsRequired();
//         builder.Property(t => t.DailyFees).IsRequired();
//         builder.Property(o => o.Days).HasComputedColumnSql($"{GetComputedDayColumn("postgresql")}", stored: true);
//         builder.Property(o => o.Amount).HasComputedColumnSql($"{GetComputedAmountColumn("postgresql")}", stored: true);
//         builder.Ignore(e => e.DomainEvents);

//     }

//     private string GetComputedDayColumn(string databaseProvider)
//     {

//         if (databaseProvider == "Sqlite")
//         {
//             return "julianday(SeDate) - julianday(SsDate)";
//         }
//         else if (databaseProvider == "postgresql")
//         {
           
//             return "\"se_date\" - \"ss_date\"";
//         }
//         else if (databaseProvider == "mssql")
//         {
//             return "DATEDIFF(day, [SsDate], [SeDate])";
//         }
//         else
//         {
//             throw new NotSupportedException("This database provider is not supported.");
//         }
//     }

//     private string GetComputedAmountColumn(string databaseProvider)
//     {

//         if (databaseProvider == "Sqlite")
//         {
//             return "julianday(SeDate) - julianday(SsDate) * DailyFees";
//         }
//         else if (databaseProvider == "postgresql")
//         {
//             return "(\"se_date\" - \"ss_date\") * daily_fees";
//         }
//         else if (databaseProvider == "mssql")
//         {
//             return "DATEDIFF(day, [SsDate], [SeDate]) * DailyFees";
//         }
//         else
//         {
//             throw new NotSupportedException("This database provider is not supported.");
//         }
//     }
// }


