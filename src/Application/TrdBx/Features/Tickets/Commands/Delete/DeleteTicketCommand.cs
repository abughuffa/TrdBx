using CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Delete;

public class DeleteTicketCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => TicketCacheKey.Tags;
    public DeleteTicketCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteTicketCommandHandler :
             IRequestHandler<DeleteTicketCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteTicketCommandHandler(
       IApplicationDbContextFactory dbContextFactory
    )
    {
       _dbContextFactory = dbContextFactory;
    }

    // private readonly IApplicationDbContext _context;
    // public DeleteTicketCommandHandler(
    //     IApplicationDbContext context
    // )
    // {
    //     _context = context;
    // }
    public async ValueTask<Result> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var items = await context.Tickets.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
           // raise a delete domain event
           item.AddDomainEvent(new TicketDeletedEvent(item));
           context.Tickets.Remove(item);
        }
        await context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();

    //     var items = await _context.Tickets.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
    //     foreach (var item in items)
    //     {
    //         // raise a delete domain event
    //         item.AddDomainEvent(new TicketDeletedEvent(item));
    //         _context.Tickets.Remove(item);
    //     }
    //    var result= await _context.SaveChangesAsync(cancellationToken);
    //     return await Result<int>.SuccessAsync(result);
    }

}

