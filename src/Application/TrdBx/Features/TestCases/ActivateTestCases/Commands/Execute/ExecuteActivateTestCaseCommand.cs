
using CleanArchitecture.Blazor.Application.Features.ActivateTestCases.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnit;
using CleanArchitecture.Blazor.Domain.Events;



namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.Commands.Execute;

public class ExecuteActivateTestCaseCommand : ICacheInvalidatorRequest<Result>
{
    public int[] Id { get; }

    internal IMediator Mediator;
    public string CacheKey => ActivateTestCaseCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => ActivateTestCaseCacheKey.Tags;
    public ExecuteActivateTestCaseCommand(int[] id, IMediator mediator)
    {
        Id = id;
        Mediator = mediator;
    }


}

public class ExecuteActivateTestCaseCommandHandler : IRequestHandler<ExecuteActivateTestCaseCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public ExecuteActivateTestCaseCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }
    public async Task<Result> Handle(ExecuteActivateTestCaseCommand request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);

        var items = await db.ActivateTestCases.Where(x => request.Id.Contains(x.Id))
             //.ProjectTo<ActivateTestCaseDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        if (items.Any(i => i.IsSucssed == true))
        {
            return await Result.FailureAsync("Some of selected Test cases already executed!");
        }

        foreach (var item in items)
        {
            var cmd = _mapper.Map<ActivateTrackingUnitCommand>(item);

            var r = await request.Mediator.Send(cmd);

            //item.CaseCode = r.D
            item.IsSucssed = r.Succeeded;

            item.Message = r.ErrorMessage;

            item.AddDomainEvent(new ActivateTestCaseUpdatedEvent(item));
        }

        await db.SaveChangesAsync(cancellationToken);

        return await Result.SuccessAsync();

    }

}

