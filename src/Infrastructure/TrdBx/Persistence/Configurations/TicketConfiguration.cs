
using CleanArchitecture.Blazor.Domain.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable
// public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
// {
//     public void Configure(EntityTypeBuilder<Ticket> builder)
//     {
//         builder.HasIndex(t => t.TicketNo).IsUnique(true);
//         builder.Property(t => t.TicketNo).HasMaxLength(50).IsRequired();
//         builder.Ignore(e => e.DomainEvents);
//         builder.HasOne(x => x.CreatedByUser)
//     .WithMany()
//     .HasForeignKey(x => x.CreatedById)
//     .OnDelete(DeleteBehavior.Restrict);
//         builder.HasOne(x => x.LastModifiedByUser)
//             .WithMany()
//             .HasForeignKey(x => x.LastModifiedById)
//             .OnDelete(DeleteBehavior.Restrict);
//         builder.Navigation(e => e.CreatedByUser).AutoInclude();
//         builder.Navigation(e => e.LastModifiedByUser).AutoInclude();
//     }
// }

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();
        
        builder.Property(e => e.TicketNo)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("TicketNo");
        
        builder.Property(e => e.ServiceTask)
            .IsRequired()
            // .HasConversion<string>()
            .HasColumnName("ServiceTask");
        
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("Description");
        
        builder.Property(e => e.TicketStatus)
            .IsRequired()
            // .HasConversion<string>()
            .HasColumnName("TicketStatus");
        
        builder.Property(e => e.TrackingUnitId)
            .IsRequired()
            .HasColumnName("TrackingUnitId");
        
        builder.Property(e => e.TcDate)
            .IsRequired()
            .HasColumnName("TcDate");
        
        builder.Property(e => e.TaDate)
            .HasColumnName("TaDate");
        
        builder.Property(e => e.TeDate)
            .HasColumnName("TeDate");
        
        builder.Property(e => e.Note)
            .HasMaxLength(512)
            .HasColumnName("Note");
        
        // Indexes
        builder.HasIndex(e => new { e.TrackingUnitId, e.TicketStatus })
            .HasDatabaseName("IX_Ticket_TrackingUnitId_TicketStatus");
        
        builder.HasIndex(e => e.TicketNo)
            .HasDatabaseName("IX_Ticket_TicketNo");
        
        builder.HasIndex(e => e.TicketStatus)
            .HasDatabaseName("IX_Ticket_TicketStatus");
        
        // Relationships
        builder.HasOne(e => e.TrackingUnit)
            .WithMany(e => e.Tickets)
            .HasForeignKey(e => e.TrackingUnitId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.CreatedByUser)
            .WithMany()
            .HasForeignKey(e => e.CreatedById)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(e => e.LastModifiedByUser)
            .WithMany()
            .HasForeignKey(e => e.LastModifiedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Navigation(e => e.CreatedByUser).AutoInclude();
        builder.Navigation(e => e.LastModifiedByUser).AutoInclude();
    }
}
