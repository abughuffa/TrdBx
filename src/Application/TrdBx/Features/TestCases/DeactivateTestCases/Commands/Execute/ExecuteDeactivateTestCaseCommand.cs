using CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Caching;
using CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Mappers;


namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Commands.Execute;

public class ExecuteDeactivateTestCaseCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }

    internal IMediator Mediator;
     public IEnumerable<string> Tags => DeactivateTestCaseCacheKey.Tags;
    public ExecuteDeactivateTestCaseCommand(int[] id, IMediator mediator)
    {
        Id = id;
        Mediator = mediator;
    }
}

public class ExecuteDeactivateTestCaseCommandHandler :
             IRequestHandler<ExecuteDeactivateTestCaseCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public ExecuteDeactivateTestCaseCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public ExecuteDeactivateTestCaseCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(ExecuteDeactivateTestCaseCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var items = await _context.DeactivateTestCases.Where(x => request.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);
        if (items.Any(i => i.IsSucssed == true))
        {
            return await Result<int>.FailureAsync("Some of selected Test cases already executed!");
        }

        foreach (var item in items)
        {



            //var cmd = _mapper.Map<DeactivateTrackingUnitCommand>(item);

            var cmd = Mapper.ToExecuteCommand(item);

            var r = await request.Mediator.Send(cmd);

            item.IsSucssed = r.Succeeded;

            item.Message = r.ErrorMessage;

            item.AddDomainEvent(new DeactivateTestCaseUpdatedEvent(item));

        }

        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);

    }

}

