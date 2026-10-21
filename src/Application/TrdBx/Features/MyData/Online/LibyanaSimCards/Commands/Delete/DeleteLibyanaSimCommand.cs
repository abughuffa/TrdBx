using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.Caching;
using CleanArchitecture.Blazor.Domain.Events;



namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.Commands.Delete;

public class DeleteLibyanaSimCardCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => LibyanaSimCardCacheKey.Tags;
    public DeleteLibyanaSimCardCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteLibyanaSimCardCommandHandler :
             IRequestHandler<DeleteLibyanaSimCardCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public DeleteLibyanaSimCardCommandHandler(
       IApplicationDbContextFactory dbContextFactory

    )
    {
       _dbContextFactory = dbContextFactory;

    }

    // private readonly IApplicationDbContext _context;
    // public DeleteLibyanaSimCardCommandHandler(
    //     IApplicationDbContext context
    // )
    // {
    //     _context = context;
    // }
    public async ValueTask<Result> Handle(DeleteLibyanaSimCardCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await context.LibyanaSimCards.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
           // raise a delete domain event
           item.AddDomainEvent(new LibyanaSimCardDeletedEvent(item));
           context.LibyanaSimCards.Remove(item);
        }
        await context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();

        // var items = await _context.LibyanaSimCards.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        // foreach (var item in items)
        // {
        //     // raise a delete domain event
        //     item.AddDomainEvent(new LibyanaSimCardDeletedEvent(item));
        //     _context.LibyanaSimCards.Remove(item);
        // }
        // var result = await _context.SaveChangesAsync(cancellationToken);

        // return await Result<int>.SuccessAsync(result);
    }

}

