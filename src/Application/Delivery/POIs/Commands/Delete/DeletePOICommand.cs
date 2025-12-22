

using CleanArchitecture.Blazor.Application.Features.POIs.Caching;


namespace CleanArchitecture.Blazor.Application.Features.POIs.Commands.Delete;

public class DeletePOICommand:  ICacheInvalidatorRequest<Result<int>>
{
  public int[] Id {  get; }
  public string CacheKey => POICacheKey.GetAllCacheKey;
  public IEnumerable<string>? Tags => POICacheKey.Tags;
  public DeletePOICommand(int[] id)
  {
    Id = id;
  }
}

public class DeletePOICommandHandler : 
             IRequestHandler<DeletePOICommand, Result<int>>

{
    private readonly IApplicationDbContext _context;
    public DeletePOICommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeletePOICommand request, CancellationToken cancellationToken)
    {
        var items = await _context.POIs.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
		    // raise a delete domain event
			item.AddDomainEvent(new POIDeletedEvent(item));
            _context.POIs.Remove(item);
        }
        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

