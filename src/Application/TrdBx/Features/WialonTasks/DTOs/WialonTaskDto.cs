using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;

[Description("WialonTasks")]
public class WialonTaskDto
{
    [Display(Name = "Id")] public int Id { get; set; }
    [Display(Name = "ServiceLogId")] public int ServiceLogId { get; set; }
    [Display(Name = "TrackingUnitId")]  public int TrackingUnitId { get; set; }
    [Display(Name = "Description")] public string Description { get; set; } = string.Empty;
    [Display(Name = "APITask")] public APITask APITask { get; set; }
    [Display(Name = "ExcDate")] public DateOnly? ExcDate { get; set; }
    [Display(Name = "IsExecuted")] public bool IsExecuted { get; set; }


    [Display(Name = "ServiceLog")] public string? ServiceLog { get; set; }
    [Display(Name = "TrackingUnit")] public string? TrackingUnit { get; set; }

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

