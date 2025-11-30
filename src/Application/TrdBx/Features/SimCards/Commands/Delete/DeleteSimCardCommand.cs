using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Delete;

public class DeleteSimCardCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => SimCardCacheKey.Tags;
    public DeleteSimCardCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteSimCardCommandHandler :
             IRequestHandler<DeleteSimCardCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    ////private readonly IMapper _mapper;
    //public DeleteSimCardCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    //_mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteSimCardCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteSimCardCommand request, CancellationToken cancellationToken)
    {

        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await db.SimCards.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new SimCardDeletedEvent(item));
        //    db.SimCards.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.SimCards.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new SimCardDeletedEvent(item));
            _context.SimCards.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

