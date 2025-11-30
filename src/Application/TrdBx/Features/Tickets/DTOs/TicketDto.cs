using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;

[Description("Tickets")]
public class TicketDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("TicketNo")]
    public string TicketNo { get; set; } = string.Empty;
    [Description("ServiceTask")]
    public ServiceTask ServiceTask { get; set; }
    [Description("Desc")]
    public string Desc { get; set; } = string.Empty;
    [Description("TicketStatus")]
    public TicketStatus TicketStatus { get; set; }
    [Description("TrackingUnitId")]
    public int TrackingUnitId { get; set; }

    [Description("TcDate")]
    public DateOnly TcDate { get; set; }

    [Description("TaDate")]
    public DateOnly? TaDate { get; set; }

    [Description("InstallerId")]
    public string? InstallerId { get; set; }
    [Description("TeDate")]
    public DateOnly? TeDate { get; set; }
    [Description("Note")]
    public string? Note { get; set; } = string.Empty;


    [Description("TrackingUnit")] public string? TrackingUnit { get; set; }
    [Description("Installer")] public string? Installer { get; set; }
    //private class Mapping : Profile
    //{
    //    //public Mapping()
    //    //{
    //    //    CreateMap<Ticket, TicketDto>(MemberList.None)
    //    //        .ForMember(dest => dest.TrackingUnit,
    //    //              opt => opt.MapFrom(src => (src.TrackingUnit == null ? null : src.TrackingUnit.SNo)))
    //    //                 .ForMember(dest => dest.Installer,
    //    //              opt => opt.MapFrom(src => (src.Installer == null ? null : src.Installer.DisplayName)));

    //    //    CreateMap<TicketDto, Ticket>(MemberList.None);
    //    //}
    //}

}

