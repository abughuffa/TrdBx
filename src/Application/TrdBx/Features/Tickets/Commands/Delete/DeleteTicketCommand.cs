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
    public async Task<Result> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var items = await _context.Tickets.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new TicketDeletedEvent(item));
            _context.Tickets.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

