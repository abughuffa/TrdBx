using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.DTOs;

[Description("TrackingUnitModels")]
public class TrackingUnitModelDto
{
    [Display(Name = "Id")] public int Id { get; set; } = 0;
    [Display(Name = "WialonName")] public string? WialonName { get; set; }
    [Display(Name = "Name")] public string? Name { get; set; }
    [Display(Name = "WhwTypeId")] public int WhwTypeId { get; set; }
    [Display(Name = "DefualtHost")] public decimal DefualtHost { get; set; } = 0.0m;
    [Display(Name = "DefualtGprs")] public decimal DefualtGprs { get; set; } = 0.0m;
    [Display(Name = "DefualtPrice")] public decimal DefualtPrice { get; set; } = 0.0m;
    [Display(Name = "PortNo1")] public int PortNo1 { get; set; }
    [Display(Name = "PortNo2")] public int PortNo2 { get; set; }
    [Display(Name = "OldId")] public int? OldId { get; set; } = null;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitModel, TrackingUnitModelDto>(MemberList.None);
    //        CreateMap<TrackingUnitModelDto, TrackingUnitModel>(MemberList.None);
    //    }
    //}


}
   

