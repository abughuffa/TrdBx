using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;
using CommunityToolkit.HighPerformance.Helpers;



namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Execute;

public class MarkAsExecutedTicketCommand : ICacheInvalidatorRequest<Result>
{
    [Description("Id")] public int Id { get; set; }

     public IEnumerable<string> Tags => TicketCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TicketDto, StartTicketCommand>(MemberList.None);
    //    }
    //}
}
public class MarkAsExecutedTicketCommandHandler : IRequestHandler<MarkAsExecutedTicketCommand, Result>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public StartTicketCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public MarkAsExecutedTicketCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(MarkAsExecutedTicketCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var ticket = await _context.Tickets.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"Ticket with id: [{request.Id}] not found.");

        if (!(ticket.TicketStatus == TicketStatus.Accepted))
        {
            return await Result.FailureAsync("Ticket Status should be Accepted to Execute it.");
        }


        ticket.TicketStatus = TicketStatus.Closed;

        ticket.AddDomainEvent(new TicketUpdatedEvent(ticket));

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return await Result.SuccessAsync();
        }
        else
            return await Result.FailureAsync("Ticket Closing Faild!");

    }
}

