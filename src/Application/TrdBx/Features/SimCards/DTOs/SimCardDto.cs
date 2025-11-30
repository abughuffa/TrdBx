using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;

[Description("SimCards")]
public class SimCardDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("SimCardNo")]
    public string SimCardNo { get; set; } = string.Empty;
    [Description("ICCID")]
    public string? ICCID { get; set; }
    [Description("SPackageId")]
    public int SPackageId { get; set; }
    [Description("SStatus")]
    public SStatus SStatus { get; set; }
    [Description("ExDate")]
    public DateOnly? ExDate { get; set; }
    [Description("OldId")]
    public int? OldId { get; set; } = null;


    [Description("SPackage")] public string? SPackage { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<SimCard, SimCardDto>(MemberList.None)
    //            .ForMember(dest => dest.SPackage,
    //                  opt => opt.MapFrom(src => (src.SPackage == null ? null : src.SPackage.Name)));
    //        CreateMap<SimCardDto, SimCard>(MemberList.None);
    //    }
    //}
}

