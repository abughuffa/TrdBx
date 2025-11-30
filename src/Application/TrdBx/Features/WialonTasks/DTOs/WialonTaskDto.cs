using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;

[Description("WialonTasks")]
public class WialonTaskDto
{
    [Description("Id")] public int Id { get; set; }
    [Description("ServiceLogId")] public int ServiceLogId { get; set; }
    [Description("TrackingUnitId")]  public int TrackingUnitId { get; set; }
    [Description("Desc")] public string Desc { get; set; } = string.Empty;
    [Description("APITask")] public APITask APITask { get; set; }
    [Description("ExcDate")] public DateOnly? ExcDate { get; set; }
    [Description("IsExecuted")] public bool IsExecuted { get; set; }


    [Description("ServiceLog")] public string? ServiceLog { get; set; }
    [Description("TrackingUnit")] public string? TrackingUnit { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<WialonTask, WialonTaskDto>(MemberList.None)
    //             .ForMember(dest => dest.ServiceLog,
    //                  opt => opt.MapFrom(src => (src.ServiceLog == null ? null : src.ServiceLog.ServiceNo)))
    //         .ForMember(dest => dest.TrackingUnit,
    //                  opt => opt.MapFrom(src => (src.TrackingUnit == null ? null : src.TrackingUnit.SNo)));

    //        CreateMap<WialonTaskDto, WialonTask>(MemberList.None);
    //    }
    //}
}

