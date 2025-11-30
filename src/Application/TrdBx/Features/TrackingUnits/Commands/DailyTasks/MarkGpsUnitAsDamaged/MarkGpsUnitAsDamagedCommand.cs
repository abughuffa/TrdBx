using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.MarkTrackingUnitAsUsed;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Events;
namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.MarkTrackingUnitAsDamaged;

public class MarkTrackingUnitAsDamagedCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, MarkTrackingUnitAsDamagedCommand>(MemberList.None)
    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            .ForMember(dest => dest.TsDate, opt => opt.Ignore());
    //    }
    //}

}

public class MarkTrackingUnitAsDamagedCommandHandler : IRequestHandler<MarkTrackingUnitAsDamagedCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<MarkTrackingUnitAsDamagedCommandHandler> _localizer;
    //public MarkTrackingUnitAsDamagedCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<MarkTrackingUnitAsDamagedCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<MarkTrackingUnitAsDamagedCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public MarkTrackingUnitAsDamagedCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<MarkTrackingUnitAsDamagedCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(MarkTrackingUnitAsDamagedCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).Include(x => x.Subscriptions).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.Recovered || unit.UStatus == UStatus.Used))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be Recovered or used to procced");
        }

        unit.UStatus = UStatus.Damaged;

        unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
            return await Result<int>.SuccessAsync(unit.Id);
        else
            return await Result<int>.FailureAsync("MarkTrackingUnitAsDamaged Faild!");




    }
}

