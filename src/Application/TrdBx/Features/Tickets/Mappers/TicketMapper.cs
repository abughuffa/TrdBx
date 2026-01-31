
using CleanArchitecture.Blazor.Application.Features.Identity.Mappers;
using CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Reject;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
[UseStaticMapper(typeof(ApplicationUserMapper))]
public static partial class Mapper
{
    [MapProperty(nameof(Ticket.TrackingUnit.SNo), nameof(TicketDto.TrackingUnit))]
    //[MapProperty(nameof(Ticket.Installer.DisplayName), nameof(TicketDto.Installer))]
    public static partial TicketDto ToDto(Ticket source);

    [MapperIgnoreSource(nameof(TicketDto.TrackingUnit))]
    [MapperIgnoreSource(nameof(TicketDto.CreatedByUser))]
    [MapperIgnoreSource(nameof(TicketDto.LastModifiedByUser))]
    public static partial Ticket FromDto(TicketDto dto);

    //public static partial AssignTicketCommand ToAssignCommand(TicketDto dto);
    public static partial RejectTicketCommand ToRejectCommand(TicketDto dto);
    //public static partial Ticket FromEditCommand(AddEditTicketCommand command);
    //public static partial Ticket FromCreateCommand(CreateTicketCommand command);
    //public static partial UpdateTicketCommand ToUpdateCommand(TicketDto dto);
    //public static partial AddEditTicketCommand CloneFromDto(TicketDto dto);
    //public static partial void ApplyChangesFrom(UpdateTicketCommand source, Ticket target);
    //public static partial void ApplyChangesFrom(AddEditTicketCommand source, Ticket target);
    public static partial IQueryable<TicketDto> ProjectTo(this IQueryable<Ticket> q);
}

