using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Caching;


namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Commands.Delete;

public class DeleteActivateGprsTestCaseCommand:  ICacheInvalidatorRequest<Result<int>>
{
  public int[] Id {  get; }
  public IEnumerable<string> Tags => ActivateGprsTestCaseCacheKey.Tags;
  public DeleteActivateGprsTestCaseCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteActivateGprsTestCaseCommandHandler : 
             IRequestHandler<DeleteActivateGprsTestCaseCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteActivateGprsTestCaseCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteActivateGprsTestCaseCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteActivateGprsTestCaseCommand request, CancellationToken cancellationToken)
    {
   //     await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
   //     var items = await _context.ActivateGprsTestCases.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
   //     foreach (var item in items)
   //     {
		 //   // raise a delete domain event
			//item.AddDomainEvent(new ActivateGprsTestCaseDeletedEvent(item));
   //         _context.ActivateGprsTestCases.Remove(item);
   //     }
   //     await _context.SaveChangesAsync(cancellationToken);
   //     return await Result.SuccessAsync();

        var items = await _context.ActivateGprsTestCases.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new ActivateGprsTestCaseDeletedEvent(item));
            _context.ActivateGprsTestCases.Remove(item);
        }
       var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }

}

