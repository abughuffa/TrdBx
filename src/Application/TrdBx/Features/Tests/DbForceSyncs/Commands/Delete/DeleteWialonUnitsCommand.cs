using CleanArchitecture.Blazor.Application.Features.WialonUnits.Caching;

namespace CleanArchitecture.Blazor.Application.Features.DbForceSyncs.Delete;
public class DeleteWialonUnitsCommand : ICacheInvalidatorRequest<Result>
{
    public IEnumerable<string> Tags => WialonUnitCacheKey.Tags;
}

public class DeleteWialonUnitsCommandHandler :
             IRequestHandler<DeleteWialonUnitsCommand, Result>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteWialonUnitsCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteWialonUnitsCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(DeleteWialonUnitsCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await _context.WialonUnits.ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new WialonUnitDeletedEvent(item));
        //    _context.WialonUnits.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.WialonUnits.ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new WialonUnitDeletedEvent(item));
            _context.WialonUnits.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

