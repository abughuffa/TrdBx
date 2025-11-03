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
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteServiceLogCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(DeleteServiceLogCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await db.ServiceLogs.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new ServiceLogDeletedEvent(item));
            db.ServiceLogs.Remove(item);
        }
        await db.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

