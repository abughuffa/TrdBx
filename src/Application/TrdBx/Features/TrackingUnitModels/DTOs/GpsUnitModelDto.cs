using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.DTOs;

[Description("TrackingUnitModels")]
public class TrackingUnitModelDto
{
    [Description("Id")] public int Id { get; set; } = 0;
    [Description("WialonName")] public string? WialonName { get; set; }
    [Description("Name")] public string? Name { get; set; }
    [Description("WhwTypeId")] public int WhwTypeId { get; set; }
    [Description("DefualtHost")] public decimal DefualtHost { get; set; } = 0.0m;
    [Description("DefualtGprs")] public decimal DefualtGprs { get; set; } = 0.0m;
    [Description("DefualtPrice")] public decimal DefualtPrice { get; set; } = 0.0m;
    [Description("PortNo1")] public int PortNo1 { get; set; }
    [Description("PortNo2")] public int PortNo2 { get; set; }
    [Description("OldId")]
    public int? OldId { get; set; } = null;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitModel, TrackingUnitModelDto>(MemberList.None);
    //        CreateMap<TrackingUnitModelDto, TrackingUnitModel>(MemberList.None);
    //    }
    //}


}
   

