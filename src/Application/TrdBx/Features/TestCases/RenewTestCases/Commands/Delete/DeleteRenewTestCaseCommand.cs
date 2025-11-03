
//using CleanArchitecture.Blazor.Application.Features.RenewTestCases.Caching;


//namespace CleanArchitecture.Blazor.Application.Features.RenewTestCases.Commands.Delete;

//public class DeleteRenewTestCaseCommand:  ICacheInvalidatorRequest<Result>
//{
//  public int[] Id {  get; }
//  public IEnumerable<string> Tags => RenewTestCaseCacheKey.Tags;
//  public DeleteRenewTestCaseCommand(int[] id)
//  {
//    Id = id;
//  }
//}

//public class DeleteRenewTestCaseCommandHandler : 
//             IRequestHandler<DeleteRenewTestCaseCommand, Result>

//{
//    private readonly IApplicationDbContext _context;
//    public DeleteRenewTestCaseCommandHandler(
//        IApplicationDbContext context)
//    {
//        _context = context;
//    }
//    public async Task<Result> Handle(DeleteRenewTestCaseCommand request, CancellationToken cancellationToken)
//    {
//        var items = await _context.RenewTestCases.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
//        foreach (var item in items)
//        {
//		    // raise a delete domain event
//			item.AddDomainEvent(new RenewTestCaseDeletedEvent(item));
//            _context.RenewTestCases.Remove(item);
//        }
//        await _context.SaveChangesAsync(cancellationToken);
//        return await Result.SuccessAsync();
//    }

//}

