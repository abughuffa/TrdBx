using CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Commands.Delete;

public class DeleteWialonTaskCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => WialonTaskCacheKey.Tags;
    public DeleteWialonTaskCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteWialonTaskCommandHandler :
             IRequestHandler<DeleteWialonTaskCommand, Result>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteWialonTaskCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteWialonTaskCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteWialonTaskCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await _context.WialonTasks.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new WialonTaskDeletedEvent(item));
        //    _context.WialonTasks.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.WialonTasks.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new WialonTaskDeletedEvent(item));
            _context.WialonTasks.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();


    }

}

