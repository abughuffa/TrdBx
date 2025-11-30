// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class TrackedAssetConfiguration : IEntityTypeConfiguration<TrackedAsset>
{
    public void Configure(EntityTypeBuilder<TrackedAsset> builder)
    {
        builder.HasIndex(t => t.TrackedAssetNo).IsUnique(true);
        builder.Property(t => t.TrackedAssetCode).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}


