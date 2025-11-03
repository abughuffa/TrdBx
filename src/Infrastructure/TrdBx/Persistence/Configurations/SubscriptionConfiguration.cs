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
        builder.Property(t => t.SsDate).HasMaxLength(256).IsRequired();
        builder.Property(o => o.Days).HasComputedColumnSql($"{GetComputedColumnSql("Sqlite")}", stored: true);
        builder.Property(o => o.Amount).HasComputedColumnSql($"({GetComputedColumnSql("Sqlite")}) * DailyFees", stored: true);
        builder.Ignore(e => e.DomainEvents);
    }

    private string GetComputedColumnSql(string databaseProvider)
    {

        if (databaseProvider == "Sqlite")
        {
            return "julianday(SeDate) - julianday(SsDate)";
        }
        else if (databaseProvider == "SqlServer")
        {
            return "DATEDIFF(day, SsDate, SeDate)";
        }
        else
        {
            throw new NotSupportedException("This database provider is not supported.");
        }
    }
}


