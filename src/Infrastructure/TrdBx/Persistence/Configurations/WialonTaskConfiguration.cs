//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class WialonTaskConfiguration : IEntityTypeConfiguration<WialonTask>
{
    public void Configure(EntityTypeBuilder<WialonTask> builder)
    {
        builder.Property(t => t.TrackingUnitId).IsRequired();
        builder.Property(t => t.Desc).HasMaxLength(256).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}



