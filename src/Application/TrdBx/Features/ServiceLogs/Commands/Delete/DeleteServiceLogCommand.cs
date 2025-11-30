using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Delete;

public class DeleteServiceLogCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }
     public IEnumerable<string> Tags => ServiceLogCacheKey.Tags;
    public DeleteServiceLogCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteServiceLogCommandHandler :
             IRequestHandler<DeleteServiceLogCommand, Result>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteServiceLogCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteServiceLogCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(DeleteServiceLogCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await _context.ServiceLogs.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new ServiceLogDeletedEvent(item));
        //    _context.ServiceLogs.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.ServiceLogs.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new ServiceLogDeletedEvent(item));
            _context.ServiceLogs.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

