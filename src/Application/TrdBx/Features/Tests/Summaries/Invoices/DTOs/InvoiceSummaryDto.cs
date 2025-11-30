using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Summaries.Invoices.DTOs;

[Description("InvoiceSummary")]
public class InvoiceSummaryDto
{
    [Description("Drafts")]
    public int Drafts { get; set; } = 0;
    [Description("SentToTaxs")]
    public int SentToTaxs { get; set; } = 0;
    [Description("Readys")]
    public int Readys { get; set; } = 0;
    [Description("Billeds")]
    public int Billeds { get; set; } = 0;
    [Description("Paids")]
    public int Paids { get; set; } = 0;
    [Description("Canceleds")]
    public int Canceleds { get; set; } = 0;

    [Description("Counts")]
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

