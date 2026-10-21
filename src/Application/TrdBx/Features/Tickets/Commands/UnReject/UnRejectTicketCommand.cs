using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.UnReject;

public class UnRejectTicketCommand : ICacheInvalidatorRequest<Result>
{
    [Display(Name = "Id")] public int Id { get; set; }
     public IEnumerable<string> Tags => TicketCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TicketDto, UnRejectTicketCommand>(MemberList.None);
    //    }
    //}
}
public class UnRejectTicketCommandHandler : IRequestHandler<UnRejectTicketCommand, Result>
{

  private readonly IApplicationDbContextFactory _dbContextFactory;
    public UnRejectTicketCommandHandler(
       IApplicationDbContextFactory dbContextFactory
    )
    {
       _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<Result> Handle(UnRejectTicketCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var ticket = await context.Tickets.Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"Ticket with id: [{request.Id}] not found.");

        if (!(ticket.TicketStatus == TicketStatus.Rejected))
        {
            return await Result.FailureAsync("Ticket Status should be Rejected to UnReject it.");
        }

        ticket.TicketStatus = TicketStatus.Opened;


        ticket.AddDomainEvent(new TicketUpdatedEvent(ticket));

        var result = await context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            return await Result.SuccessAsync();
        }
        else
            return await Result.FailureAsync("Ticket UnRejecation Faild!");

    }
}

