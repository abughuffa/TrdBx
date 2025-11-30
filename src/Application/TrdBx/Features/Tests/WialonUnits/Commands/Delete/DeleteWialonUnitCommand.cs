
using CleanArchitecture.Blazor.Application.Features.WialonUnits.Caching;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.WialonUnits.Commands.Delete;

public class DeleteWialonUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => WialonUnitCacheKey.Tags;
    public DeleteWialonUnitCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteWialonUnitCommandHandler :
             IRequestHandler<DeleteWialonUnitCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    
    //public DeleteWialonUnitCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
        
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
        
    //}

    private readonly IApplicationDbContext _context;
    public DeleteWialonUnitCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteWialonUnitCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await _context.WialonUnits.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new WialonUnitDeletedEvent(item));
        //    _context.WialonUnits.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.WialonUnits.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new WialonUnitDeletedEvent(item));
            _context.WialonUnits.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

