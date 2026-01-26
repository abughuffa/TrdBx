using CleanArchitecture.Blazor.Application.TrdBx.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;

[Description("Invoices")]
public class InvoiceDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("InvoiceNo")]
    public string InvoiceNo { get; set; } = string.Empty;
    [Description("InvoiceDate")]
    public DateOnly InvoiceDate { get; set; }
    [Description("DueDate")]
    public DateOnly DueDate { get; set; }
    [Description("Payment Date")]
    public DateOnly? PaymentDate { get; set; }
    [Description("Paid Amount")]
    public decimal PaidAmount { get; set; } = 0.0m;
    [Description("Invoice Type")]
    public InvoiceType InvoiceType { get; set; }
    [Description("IStatus")]
    public IStatus IStatus { get; set; }
    [Description("CustomerId")]
    public int CustomerId { get; set; }
    [Description("DisplayCusName")]
    public string DisplayCusName { get; set; } = string.Empty;
    [Description("Description")]
    public string Description { get; set; } = string.Empty;
    [Description("IsTaxable?")]
    public bool IsTaxable { get; set; } = false;
    [Description("IsTaxIgnored?")]
    public bool IsTaxIgnored { get; set; } = true;
    
    [Description("Total")]
    public decimal Total { get; set; } = 0.0m;

    //*************************************//

    [Description("Discount Rate")]
    public decimal DiscountRate { get; set; } = 0.0m;
    [Description("Discount")]
    public decimal DiscountAmount { get; set; } = 0.0m;
    [Description("Tax Rate")]
    public decimal TaxRate { get; set; } = 1.0m;
    [Description("Tax")]
    public decimal TaxAmount { get; set; } = 0.0m;
    [Description("Taxable Amount")]
    public decimal TaxableAmount { get; set; } = 0.0m;
    [Description("Grand Total")]
    public decimal GrandTotal { get; set; } = 0.0m;

    [Description("Customer")] public string? Customer { get; set; }

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

