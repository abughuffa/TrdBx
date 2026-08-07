using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;



namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Reject;

public class RejectTicketCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("Note")] public string Note { get; set; } = string.Empty;

     public IEnumerable<string> Tags => TicketCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TicketDto, RejectTicketCommand>(MemberList.None)
    //            .ForMember(dest => dest.Note, opt => opt.Ignore());
    //    }
    //}
}
public class RejectTicketCommandHandler : IRequestHandler<RejectTicketCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public RejectTicketCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public RejectTicketCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(RejectTicketCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var ticket = await _context.Tickets.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"Ticket with id: [{request.Id}] not found.");

        if (!(ticket.TicketStatus == TicketStatus.Opened || ticket.TicketStatus == TicketStatus.Accepted))
        {
            return await Result<int>.FailureAsync("Ticket Status should be Opened or Accepted to Reject it.");
        }

        ticket.TicketStatus = TicketStatus.Rejected;
        //ticket.InstallerId = string.Empty;
        ticket.TaDate = null;
        ticket.Note = request.Note;

        ticket.AddDomainEvent(new TicketUpdatedEvent(ticket));

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return await Result<int>.SuccessAsync(ticket.Id);
        }
        else
            return await Result<int>.FailureAsync("Ticket Rejecation Faild!");

    }
}

