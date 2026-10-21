using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;

#nullable disable

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.Ignore(e => e.DomainEvents);
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .UseIdentityColumn();
        
        builder.Property(e => e.InvoiceNo)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("InvoiceNo");
        
        builder.Property(e => e.InvoiceDate)
            .IsRequired()
            .HasColumnName("InvoiceDate");
        
        builder.Property(e => e.DueDate)
            .IsRequired()
            .HasColumnName("DueDate");
        
        builder.Property(e => e.PaymentDate)
            .HasColumnName("PaymentDate");
        
        builder.Property(e => e.PaidAmount)
            .IsRequired()
            .HasPrecision(9, 3)
            .HasColumnName("PaidAmount");
        
        builder.Property(e => e.InvoiceType)
            .IsRequired()
            // .HasConversion<string>()
            .HasColumnName("InvoiceType");
        
        builder.Property(e => e.IStatus)
            .IsRequired()
            // .HasConversion<string>()
            .HasColumnName("IStatus");
        
        builder.Property(e => e.DisplayCusName)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("DisplayCusName");
        
        builder.Property(e => e.CustomerId)
            .IsRequired()
            .HasColumnName("CustomerId");
        
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnName("Description");
        
        builder.Property(e => e.IsTaxable)
            .IsRequired()
            .HasColumnName("IsTaxable");
        
        builder.Property(e => e.IsTaxIgnored)
            .IsRequired()
            .HasColumnName("IsTaxIgnored");
        
        builder.Property(e => e.Total)
            .IsRequired()
           .HasPrecision(9, 3)
            .HasColumnName("Total");
        
        builder.Property(e => e.DiscountRate)
            .IsRequired()
            .HasPrecision(9, 3)
            .HasColumnName("DiscountRate");
        
        builder.Property(e => e.DiscountAmount)
            .IsRequired()
            .HasPrecision(9, 3)
            .HasColumnName("DiscountAmount");
        
        builder.Property(e => e.TaxRate)
            .IsRequired()
            .HasPrecision(9, 3)
            .HasColumnName("TaxRate");
        
        builder.Property(e => e.TaxAmount)
            .IsRequired()
           .HasPrecision(9, 3)
            .HasColumnName("TaxAmount");
        
        builder.Property(e => e.TaxableAmount)
            .IsRequired()
            .HasPrecision(9, 3)
            .HasColumnName("TaxableAmount");
        
        builder.Property(e => e.GrandTotal)
            .IsRequired()
            .HasPrecision(9, 3)
            .HasColumnName("GrandTotal");
        
        // Indexes
        builder.HasIndex(e => e.InvoiceNo)
            .HasDatabaseName("IX_Invoice_InvoiceNo");
        
        builder.HasIndex(e => new { e.CustomerId, e.IStatus, e.InvoiceDate })
            .HasDatabaseName("IX_Invoice_CustomerId_IStatus_InvoiceDate");
        
        builder.HasIndex(e => e.IStatus)
            .HasDatabaseName("IX_Invoice_IStatus");
        
        builder.HasIndex(e => e.InvoiceDate)
            .HasDatabaseName("IX_Invoice_InvoiceDate");
        
        // Relationships
        builder.HasOne(e => e.Customer)
            .WithMany(e => e.Invoices)
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(e => e.InvoiceItemGroups)
            .WithOne(e => e.Invoice)
            .HasForeignKey(e => e.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}