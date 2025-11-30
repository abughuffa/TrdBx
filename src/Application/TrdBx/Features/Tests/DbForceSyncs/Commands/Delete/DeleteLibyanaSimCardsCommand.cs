using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Caching;

namespace CleanArchitecture.Blazor.Application.Features.DbForceSyncs.Delete;


public class DeleteLibyanaSimCardsCommand : ICacheInvalidatorRequest<Result>
{
    public IEnumerable<string> Tags => LibyanaSimCardCacheKey.Tags;
}

public class DeleteLibyanaSimCardsCommandHandler :
             IRequestHandler<DeleteLibyanaSimCardsCommand, Result>

{

    

    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteLibyanaSimCardsCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteLibyanaSimCardsCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(DeleteLibyanaSimCardsCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await _context.LibyanaSimCards.ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new LibyanaSimCardDeletedEvent(item));
        //    _context.LibyanaSimCards.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.LibyanaSimCards.ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new LibyanaSimCardDeletedEvent(item));
            _context.LibyanaSimCards.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

