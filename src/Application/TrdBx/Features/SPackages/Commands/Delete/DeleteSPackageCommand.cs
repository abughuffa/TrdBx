using CleanArchitecture.Blazor.Application.Features.SPackages.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.SPackages.Commands.Delete;

public class DeleteSPackageCommand:  ICacheInvalidatorRequest<Result<int>>
{
  public int[] Id {  get; }
  public IEnumerable<string> Tags => SPackageCacheKey.Tags;
  public DeleteSPackageCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteSPackageCommandHandler : 
             IRequestHandler<DeleteSPackageCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteSPackageCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteSPackageCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteSPackageCommand request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var items = await db.SPackages.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        //foreach (var item in items)
        //{
        //    // raise a delete domain event
        //    item.AddDomainEvent(new SPackageDeletedEvent(item));
        //    db.SPackages.Remove(item);
        //}
        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();

        var items = await _context.SPackages.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new SPackageDeletedEvent(item));
            _context.SPackages.Remove(item);
        }
       int result =  await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

