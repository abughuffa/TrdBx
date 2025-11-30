using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;

[Description("Invoices")]
public class InvoiceDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("InvNo")]
    public string InvNo { get; set; } = string.Empty;
    [Description("InvDate")]
    public DateOnly InvDate { get; set; }
    [Description("DueDate")]
    public DateOnly DueDate { get; set; }


    [Description("Invoice Type")]
    public InvoiceType InvoiceType { get; set; }

    [Description("IStatus")]
    public IStatus IStatus { get; set; }
    [Description("CustomerId")]
    public int CustomerId { get; set; }
    [Description("InvDesc")]
    public string InvDesc { get; set; } = string.Empty;
    [Description("Total")]
    public decimal Total { get; set; } = 0.0m;
    [Description("Taxes")]
    public decimal Taxes { get; set; } = 0.0m;
    [Description("GrangTotal")]
    public decimal GrangTotal { get; set; } = 0.0m;

    [Description("Customer")] public string? Customer { get; set; }

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

