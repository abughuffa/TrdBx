using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Caching;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Mappers;


namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Commands.Execute;

public class ExecuteActivateGprsTestCaseCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int[] Id { get; }

    internal IMediator Mediator;
     public IEnumerable<string> Tags => ActivateGprsTestCaseCacheKey.Tags;

    public ExecuteActivateGprsTestCaseCommand(int[] id, IMediator mediator)
    {
        Id = id;
        Mediator = mediator;
    }



}

public class ExecuteActivateGprsTestCaseCommandHandler :
             IRequestHandler<ExecuteActivateGprsTestCaseCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public ExecuteActivateGprsTestCaseCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public ExecuteActivateGprsTestCaseCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(ExecuteActivateGprsTestCaseCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var items = await _context.ActivateGprsTestCases.Where(x => request.Id.Contains(x.Id))
            //.ProjectTo<ActivateGprsTestCaseDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (items.Any(i => i.IsSucssed == true))
        {
            return await Result<int>.FailureAsync("Some of selected Test cases already executed!");
        }

        foreach (var item in items)
        {

            //var cmd = _mapper.Map<ActivateTrackingUnitForGprsCommand>(item);

            var cmd = Mapper.ToExecuteCommand(item);

            var r = await request.Mediator.Send(cmd);

            

            item.IsSucssed = r.Succeeded;

            item.Message = r.ErrorMessage;

            item.AddDomainEvent(new ActivateGprsTestCaseUpdatedEvent(item));
        }

        var result = await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(result);

    }

}

