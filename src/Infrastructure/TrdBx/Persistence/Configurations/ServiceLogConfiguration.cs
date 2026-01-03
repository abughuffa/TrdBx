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
        builder.HasIndex(t => t.ServiceNo).IsUnique(true);
        builder.Property(t => t.Desc).HasMaxLength(256).IsRequired();
        builder.Ignore(e => e.DomainEvents);
        builder.HasOne(x => x.CreatedByUser)
    .WithMany()
    .HasForeignKey(x => x.CreatedBy)
    .OnDelete(DeleteBehavior.Restrict);
        builder.Navigation(e => e.CreatedByUser).AutoInclude();
    }
}



