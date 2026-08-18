namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.SyncNames;

public class SyncNamesCommand : IRequest<Result<int>>
{
}
public class SyncNamesCommandHandler : IRequestHandler<SyncNamesCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public SyncNamesCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    public SyncNamesCommandHandler(
       IApplicationDbContextFactory dbContextFactory
    )
    {
       _dbContextFactory = dbContextFactory;
       //_mapper = mapper;
    }
    public async ValueTask<Result<int>> Handle(SyncNamesCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var wialon = await context.WialonUnits.ToListAsync(cancellationToken);

        if (!wialon.Any()) return await Result<int>.FailureAsync("Thier is no Wialon units imported!");

            var units = await context.TrackingUnits.ToListAsync(cancellationToken);
            foreach (var unit in units)
            {
                var wunit = wialon.Find(w=>w.UnitSNo == unit.SNo);
                unit.UnitName = wunit is not null ? wunit.UnitName != null ? wunit.UnitName : null : null;
                unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
            }
            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(1);
    }
}
