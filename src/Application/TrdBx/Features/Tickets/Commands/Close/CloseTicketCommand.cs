using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;



namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Close;

public class CloseTicketCommand : ICacheInvalidatorRequest<Result>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TeDate")] public DateOnly TeDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

     public IEnumerable<string> Tags => TicketCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TicketDto, CompleteTicketCommand>(MemberList.None)
    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            .ForMember(dest => dest.TeDate, opt => opt.Ignore());
    //    }
    //}
}
public class CloseTicketCommandHandler : IRequestHandler<CloseTicketCommand, Result>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public CompleteTicketCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public CloseTicketCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(CloseTicketCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var ticket = await _context.Tickets.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"Ticket with id: [{request.Id}] not found.");

        if (!(ticket.TicketStatus == TicketStatus.OnProcess))
        {
            return await Result.FailureAsync("Ticket Status should be OnProcess to Close it.");
        }

        ticket.TicketStatus = TicketStatus.Closed;
        ticket.TeDate = request.TeDate;
       




        ticket.AddDomainEvent(new TicketUpdatedEvent(ticket));

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return await Result.SuccessAsync();
        }
        else
            return await Result.FailureAsync("Closing Ticket were faild!");

    }
}

