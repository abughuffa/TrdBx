using CleanArchitecture.Blazor.Application.Features.SPackages.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.SPackages.Commands.Delete;

public class DeleteSPackageCommand:  ICacheInvalidatorRequest<Result>
{
  public int[] Id {  get; }
  public IEnumerable<string> Tags => SPackageCacheKey.Tags;
  public DeleteSPackageCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteSPackageCommandHandler : 
             IRequestHandler<DeleteSPackageCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteSPackageCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(DeleteSPackageCommand request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await db.SPackages.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new SPackageDeletedEvent(item));
            db.SPackages.Remove(item);
        }
        await db.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

