using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.Invoices.DTOs;

[Description("InvoiceSummary")]
public class InvoiceSummaryDto
{

    [Display(Name = "Drafts")]
    public int Drafts { get; set; } = 0;

    [Display(Name = "Sent to Tax")]
    public int SentToTaxs { get; set; } = 0;

    [Display(Name = "Ready")]
    public int Readys { get; set; } = 0;

    [Display(Name = "Billed")]
    public int Billeds { get; set; } = 0;

    [Display(Name = "Paid")]
    public int Paids { get; set; } = 0;

    [Display(Name = "Canceled")]            
    public int Canceleds { get; set; } = 0;


     [Display(Name = "Counts")]  
    public int Counts { get; set; } = 0;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<InvoiceSummary, InvoiceSummaryDto>(MemberList.None);
    //        CreateMap<InvoiceSummaryDto, InvoiceSummary>(MemberList.None);
    //    }
    //}


}

