using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Recover;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Events;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.MarkTrackingUnitAsUsed;

public class MarkTrackingUnitAsUsedCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, MarkTrackingUnitAsUsedCommand>(MemberList.None)
    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            .ForMember(dest => dest.TsDate, opt => opt.Ignore());
    //    }
    //}

}

public class MarkTrackingUnitAsUsedCommandHandler : IRequestHandler<MarkTrackingUnitAsUsedCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<MarkTrackingUnitAsUsedCommandHandler> _localizer;
    //public MarkTrackingUnitAsUsedCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<MarkTrackingUnitAsUsedCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<MarkTrackingUnitAsUsedCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public MarkTrackingUnitAsUsedCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<MarkTrackingUnitAsUsedCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(MarkTrackingUnitAsUsedCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).Include(x => x.Subscriptions).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.Recovered || unit.UStatus == UStatus.Damaged))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be Recovered or Damaged to procced");
        }

        unit.UStatus = UStatus.Used;

        unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
            return await Result<int>.SuccessAsync(unit.Id);
        else
            return await Result<int>.FailureAsync("MarkTrackingUnitAsUsed Faild!");




    }
}

