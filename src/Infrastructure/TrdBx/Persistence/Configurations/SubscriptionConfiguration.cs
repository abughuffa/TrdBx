// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.Property(t => t.TrackingUnitId).IsRequired();
        builder.Property(t => t.Desc).HasMaxLength(256).IsRequired();
        builder.Property(t => t.SsDate).IsRequired();
        builder.Property(t => t.SeDate).IsRequired();
        builder.Property(t => t.DailyFees).IsRequired();
        builder.Property(o => o.Days).HasComputedColumnSql($"{GetComputedDayColumn("postgresql")}", stored: true);
        builder.Property(o => o.Amount).HasComputedColumnSql($"{GetComputedAmountColumn("postgresql")}", stored: true);
        builder.Ignore(e => e.DomainEvents);



        //    .HasComputedColumnSql(null) // Not computed in DB
        //.ValueGeneratedOnAddOrUpdate(); // Mark as computed;//.HasComputedColumnSql($"{GetComputedColumnSql("postgresql")}", stored: true);
        //builder.Property(o => o.Amount).HasComputedColumnSql(null) // Not computed in DB
        //.ValueGeneratedOnAddOrUpdate(); // Mark as computed//.HasComputedColumnSql($"({GetComputedColumnSql("postgresql")}) * daily_fees", stored: true);
        //.HasComputedColumnSql(, stored: true);
    }

    private string GetComputedDayColumn(string databaseProvider)
    {

        if (databaseProvider == "Sqlite")
        {
            return "julianday(SeDate) - julianday(SsDate)";
        }
        else if (databaseProvider == "postgresql")
        {
           
            return "\"se_date\" - \"ss_date\"";
        }
        else if (databaseProvider == "mssql")
        {
            return "DATEDIFF(day, [SsDate], [SeDate])";
        }
        else
        {
            throw new NotSupportedException("This database provider is not supported.");
        }
    }

    private string GetComputedAmountColumn(string databaseProvider)
    {

        if (databaseProvider == "Sqlite")
        {
            return "julianday(SeDate) - julianday(SsDate) * DailyFees";
        }
        else if (databaseProvider == "postgresql")
        {
            return "(\"se_date\" - \"ss_date\") * daily_fees";
        }
        else if (databaseProvider == "mssql")
        {
            return "DATEDIFF(day, [SsDate], [SeDate]) * DailyFees";
        }
        else
        {
            throw new NotSupportedException("This database provider is not supported.");
        }
    }
}


