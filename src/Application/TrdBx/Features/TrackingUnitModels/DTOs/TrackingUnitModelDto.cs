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
    [Display(Name = "DefaultHost")] public decimal DefaultHost { get; set; } = 0.0m;
    [Display(Name = "DefaultGprs")] public decimal DefaultGprs { get; set; } = 0.0m;
    [Display(Name = "DefaultPrice")] public decimal DefaultPrice { get; set; } = 0.0m;
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
   

