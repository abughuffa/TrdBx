
//using CleanArchitecture.Blazor.Application.Features.Tickets.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Assign;
using CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Reject;

//using CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial TicketDto ToDto(Ticket source);

    [MapperIgnoreSource(nameof(TicketDto.TrackingUnit))]
    public static partial Ticket FromDto(TicketDto dto);

    public static partial AssignTicketCommand ToAssignCommand(TicketDto dto);
    public static partial RejectTicketCommand ToRejectCommand(TicketDto dto);
    //public static partial Ticket FromEditCommand(AddEditTicketCommand command);
    //public static partial Ticket FromCreateCommand(CreateTicketCommand command);
    //public static partial UpdateTicketCommand ToUpdateCommand(TicketDto dto);
    //public static partial AddEditTicketCommand CloneFromDto(TicketDto dto);
    //public static partial void ApplyChangesFrom(UpdateTicketCommand source, Ticket target);
    //public static partial void ApplyChangesFrom(AddEditTicketCommand source, Ticket target);
    public static partial IQueryable<TicketDto> ProjectTo(this IQueryable<Ticket> q);
}

