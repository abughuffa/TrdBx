using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Caching;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Commands.Delete;

public class DeleteActivateGprsTestCaseCommand:  ICacheInvalidatorRequest<Result>
{
  public int[] Id {  get; }
  public IEnumerable<string> Tags => ActivateGprsTestCaseCacheKey.Tags;
  public DeleteActivateGprsTestCaseCommand(int[] id)
  {
    Id = id;
  }
}

public class DeleteActivateGprsTestCaseCommandHandler : 
             IRequestHandler<DeleteActivateGprsTestCaseCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteActivateGprsTestCaseCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(DeleteActivateGprsTestCaseCommand request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var items = await _context.ActivateGprsTestCases.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
		    // raise a delete domain event
			item.AddDomainEvent(new ActivateGprsTestCaseDeletedEvent(item));
            _context.ActivateGprsTestCases.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

