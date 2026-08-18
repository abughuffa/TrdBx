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
        builder.HasIndex(t => t.Name).IsUnique(true);
        builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}


