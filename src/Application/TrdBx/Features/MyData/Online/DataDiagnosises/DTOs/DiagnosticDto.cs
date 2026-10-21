using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.DTOs;

[Description("DataDiagnosis")]
public class DataDiagnosisDto
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Account")]
    public string? Account { get; set; }
    [Display(Name = "Client")]
    public string? Client { get; set; }
    [Display(Name = "Customer")]
    public string? Customer { get; set; }

    [Display(Name = "UnitSNo")]
    public string? UnitSNo { get; set; }

    [Display(Name = "SimCardNo")]
    public string? SimCardNo { get; set; }

    [Display(Name = "StatusOnWialon")]
    public string? StatusOnWialon { get; set; }

    [Display(Name = "StatusOnTrdBx")]
    public string? StatusOnTrdBx { get; set; }

    //[Description("StatusOnTrdBx")]
    //public UStatus StatusOnTrdBx { get; set; }

   [Display(Name = "SimCardStatus")]
    public string? SimCardStatus { get; set; }
    [Display(Name = "LDExDate")]
    public DateTime? LDExDate { get; set; }
    [Display(Name = "LDOExpired")]
    public DateTime? LDOExpired { get; set; }
    //[Description("TNote")]
    //public string? TNote { get; set; }
    [Display(Name = "WNote")]
    public string? WNote { get; set; }
    [Display(Name = "Balance")]
    public decimal? Balance { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<DataDiagnosis, DataDiagnosisDto>(MemberList.None);
    //    }
    //}

}




