// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using CleanArchitecture.Blazor.Domain.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasIndex(t => t.TicketNo).IsUnique(true);
        builder.Property(t => t.TicketNo).HasMaxLength(50).IsRequired();
        builder.Ignore(e => e.DomainEvents);
        builder.HasOne(x => x.CreatedByUser)
    .WithMany()
    .HasForeignKey(x => x.CreatedBy)
    .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.LastModifiedByUser)
            .WithMany()
            .HasForeignKey(x => x.LastModifiedBy)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Navigation(e => e.CreatedByUser).AutoInclude();
        builder.Navigation(e => e.LastModifiedByUser).AutoInclude();
    }
}


