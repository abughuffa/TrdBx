using System.ComponentModel.DataAnnotations;
// using CleanArchitecture.Blazor.Application.TrdBx.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;

[Description("Invoices")]
public class InvoiceDto
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "InvoiceNo")]
    public string InvoiceNo { get; set; } = string.Empty;
    [Display(Name = "InvoiceDate")]
    public DateOnly InvoiceDate { get; set; }
    [Display(Name = "DueDate")]
    public DateOnly DueDate { get; set; }
    [Display(Name = "Payment Date")]
    public DateOnly? PaymentDate { get; set; }
    [Display(Name = "Paid Amount")]
    public decimal PaidAmount { get; set; } = 0.0m;
    [Display(Name = "Invoice Type")]
    public InvoiceType InvoiceType { get; set; }
    [Display(Name = "IStatus")]
    public IStatus IStatus { get; set; }
    [Display(Name = "CustomerId")]
    public int CustomerId { get; set; }
    [Display(Name = "DisplayCusName")]
    public string DisplayCusName { get; set; } = string.Empty;
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
    [Display(Name = "IsTaxable?")]
    public bool IsTaxable { get; set; } = false;
    [Display(Name = "IsTaxIgnored?")]
    public bool IsTaxIgnored { get; set; } = true;
   [Display(Name = "Total")]
    public decimal Total { get; set; } = 0.0m;

    //*************************************//

    [Display(Name = "Discount Rate")]
    public decimal DiscountRate { get; set; } = 0.0m;
    [Display(Name = "Discount")]
    public decimal DiscountAmount { get; set; } = 0.0m;
    [Display(Name = "Tax Rate")]
    public decimal TaxRate { get; set; } = 1.0m;
    [Display(Name = "Tax")]
    public decimal TaxAmount { get; set; } = 0.0m;
    [Display(Name = "Taxable Amount")]
    public decimal TaxableAmount { get; set; } = 0.0m;
    [Display(Name = "Grand Total")]
    public decimal GrandTotal { get; set; } = 0.0m;

    [Display(Name = "Customer")] public string? Customer { get; set; }

    //[Description("Customer")] public string? Customer { get; set; }

    public List<InvoiceItemGroupDto>? InvoiceItemGroups { get; set; } = null;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<Invoice, InvoiceDto>(MemberList.None)
    //            .ForMember(dest => dest.Customer,
    //                  opt => opt.MapFrom(src => (src.Customer == null ? null : src.Customer.Name)));
    //        CreateMap<InvoiceDto, Invoice>(MemberList.None);
    //    }
    //}

}

