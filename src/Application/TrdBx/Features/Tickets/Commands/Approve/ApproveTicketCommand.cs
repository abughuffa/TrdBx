using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;



namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Approve;

public class ApproveTicketCommand : ICacheInvalidatorRequest<Result>
{
    [Description("Id")] public int Id { get; set; }

     public IEnumerable<string> Tags => TicketCacheKey.Tags;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TicketDto, ApproveTicketCommand>(MemberList.None);
        }
    }
}
public class ApproveTicketCommandHandler : IRequestHandler<ApproveTicketCommand, Result>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public ApproveTicketCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(ApproveTicketCommand request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var ticket = await _context.Tickets.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"Ticket with id: [{request.Id}] not found.");

        if (!(ticket.TicketStatus == TicketStatus.JustCreated))
        {
            return await Result.FailureAsync("Ticket Status should be JustCreated to Approve it.");
        }

        ticket.TicketStatus = TicketStatus.Approved;
        ticket.Note = string.Empty;


        ticket.AddDomainEvent(new TicketUpdatedEvent(ticket));

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return await Result.SuccessAsync();
        }
        else
            return await Result.FailureAsync("Ticket Approvement Faild!");

    }
}

