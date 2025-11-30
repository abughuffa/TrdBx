using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;



namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Stop;

public class StopTicketCommand : ICacheInvalidatorRequest<Result>
{
    [Description("Id")] public int Id { get; set; }

     public IEnumerable<string> Tags => TicketCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TicketDto, StopTicketCommand>(MemberList.None);
    //    }
    //}
}
public class StopTicketCommandHandler : IRequestHandler<StopTicketCommand, Result>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public StopTicketCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public StopTicketCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(StopTicketCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var ticket = await _context.Tickets.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"Ticket with id: [{request.Id}] not found.");

        if (!(ticket.TicketStatus == TicketStatus.OnProcess ))
        {
            return await Result.FailureAsync("Ticket Status should be OnProcess to Stop it.");
        }

        ticket.TicketStatus = TicketStatus.Assigned;




        ticket.AddDomainEvent(new TicketUpdatedEvent(ticket));

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return await Result.SuccessAsync();
        }
        else
            return await Result.FailureAsync("Ticket Stopment Faild!");

    }
}

