
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Diagnostics.DTOs;

[Description("Diagnostics")]
public class DiagnosticDto
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("Account")]
    public string? Account { get; set; }
    [Description("Client")]
    public string? Client { get; set; }
    [Description("Customer")]
    public string? Customer { get; set; }

    [Description("UnitSNo")]
    public string? UnitSNo { get; set; }

    [Description("SimCardNo")]
    public string? SimCardNo { get; set; }

    [Description("StatusOnWialon")]
    public string? StatusOnWialon { get; set; }

    [Description("StatusOnTrdBx")]
    public string? StatusOnTrdBx { get; set; }

    //[Description("StatusOnTrdBx")]
    //public UStatus StatusOnTrdBx { get; set; }

    [Description("SimCardStatus")]
    public string? SimCardStatus { get; set; }
    [Description("LDExDate")]
    public DateTime? LDExDate { get; set; }
    [Description("LDOExpired")]
    public DateTime? LDOExpired { get; set; }
    //[Description("TNote")]
    //public string? TNote { get; set; }
    [Description("WNote")]
    public string? WNote { get; set; }
    [Description("Balance")]
    public decimal? Balance { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<Diagnostic, DiagnosticDto>(MemberList.None);
    //    }
    //}

}




