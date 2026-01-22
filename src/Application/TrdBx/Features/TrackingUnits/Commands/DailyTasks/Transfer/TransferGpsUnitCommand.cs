using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Transfer;

public class TransferTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Description("SimCardId")] public int SimCardId { get; set; }
    [Description("TrackedAssetId")] public int TrackedAssetId { get; set; }
    [Description("CustomerId")] public int CustomerId { get; set; }
    //[Description("InstallerId")] public string InstallerId { get; set; } = string.Empty;
    [Description("SubPackage")] public SubPackage SubPackage { get; set; } = SubPackage.Active;
    [Description("InsMode")] public InsMode InsMode { get; set; }
    [Description("CreateDeservedServices")] public bool CreateDeservedServices { get; set; } = true;
    [Description("ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;

    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, TransferTrackingUnitCommand>(MemberList.None)
    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            .ForMember(dest => dest.TsDate, opt => opt.Ignore())
    //            //.ForMember(dest => dest.SimCardId, opt => opt.Ignore())
    //            //.ForMember(dest => dest.TrackedAssetId, opt => opt.Ignore())
    //            //.ForMember(dest => dest.CustomerId, opt => opt.Ignore())
    //            .ForMember(dest => dest.InstallerId, opt => opt.Ignore())
    //            .ForMember(dest => dest.SubPackage, opt => opt.Ignore())
    //            //.ForMember(dest => dest.InsMode, opt => opt.Ignore())
    //            .ForMember(dest => dest.CreateDeservedServices, opt => opt.Ignore())
    //            .ForMember(dest => dest.ApplyChangesOnWialon, opt => opt.Ignore());
    //    }
    //}
}

public class TransferTrackingUnitCommandHandler : SubscriptionSharedLogic, IRequestHandler<TransferTrackingUnitCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<TransferTrackingUnitCommandHandler> _localizer;
    //private readonly IWialonService _wialonService;
    //public TransferTrackingUnitCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<TransferTrackingUnitCommandHandler> localizer,
    //                                     IWialonService wialonService)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _wialonService = wialonService;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<TransferTrackingUnitCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public TransferTrackingUnitCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<TransferTrackingUnitCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(TransferTrackingUnitCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await _context.TrackingUnits.Where(x => x.Id == request.Id).Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).FirstAsync(cancellationToken) ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.InstalledActive || unit.UStatus == UStatus.InstalledActiveHosting || unit.UStatus == UStatus.InstalledInactive))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be Installed to Transfer it");
        }

        var sim = await _context.SimCards.FindAsync(new object[] { request.SimCardId }, cancellationToken) ?? throw new NotFoundException($"SimCard with id: [{request.SimCardId}] not found.");

        var asset = await _context.TrackedAssets.FindAsync(new object[] { request.TrackedAssetId }, cancellationToken) ?? throw new NotFoundException($"TrackedAsset with id: [{request.TrackedAssetId}] not found.");

        var price = await GetCPrice(_context, (int)unit.CustomerId, unit.TrackingUnitModelId);

        var serviceNo = await GenSerialNo(_context, "ServiceLog", request.TsDate);

        var serviceLog = new ServiceLog()
        {
            ServiceNo = serviceNo,
            ServiceTask = ServiceTask.Transfer,
            CustomerId = request.CustomerId,
            //InstallerId = request.InstallerId,
            SerDate = request.TsDate,
            IsBilled = false,
            Amount = await GetSPrice(_context, ServiceTask.Transfer),
            Subscriptions = [],
            WialonTasks = []
        };

        if (unit.SimCardId != null && sim.Id != unit.SimCardId)
        {
            var oldSimCard = _context.SimCards.Where(a => a.Id == (int)unit.SimCardId).FirstOrDefault();
            oldSimCard.SStatus = SStatus.Recovered; //Set as Recovered
            oldSimCard.AddDomainEvent(new SimCardUpdatedEvent(oldSimCard));

            sim.SStatus = SStatus.Installed; //Set as Installed
            sim.AddDomainEvent(new SimCardUpdatedEvent(sim));
        }


        var oasset = _context.TrackedAssets.Where(a => a.Id == (int)unit.TrackedAssetId).FirstOrDefault();

        oasset.IsAvaliable = true;

        oasset.AddDomainEvent(new TrackedAssetUpdatedEvent(oasset));

        serviceLog.Desc = string.Format("نقل الوحدة ({0}) من الاصل ({1}) إلى الاصل ({2}).", unit.SNo, oasset.TrackedAssetNo, asset.TrackedAssetNo);
        serviceLog.IsDeserved = request.CreateDeservedServices;


        asset.IsAvaliable = false;
        asset.AddDomainEvent(new TrackedAssetUpdatedEvent(asset));

        unit.UnitName = asset.TrackedAssetCode;
        unit.CustomerId = request.CustomerId;
        unit.TrackedAssetId = request.TrackedAssetId;
        unit.SimCardId = request.SimCardId;
        unit.InsMode = request.InsMode;

        switch (request.SubPackage)
        {
            case SubPackage.Active:
                {
                    if (unit.UStatus != UStatus.InstalledActive)
                    {
                        Activate(unit, serviceLog, request.TsDate, price, true);
                    }
                    break;
                }
            case SubPackage.ActiveHosting:
                {
                    if (unit.UStatus != UStatus.InstalledActiveHosting)
                    {
                        ActivateForHosting(unit, serviceLog, request.TsDate, price, true);
                    }
                    break;
                }
            case SubPackage.ActiveGprs:
                {
                    if (unit.UStatus != UStatus.InstalledActiveGprs)
                    {
                        ActivateForGprs(unit, serviceLog, request.TsDate, price, true);
                    }
                    break;
                }
        }


        //serviceLog.Amount += 0.0m;

        serviceLog.AddDomainEvent(new ServiceLogCreatedEvent(serviceLog));

        _context.ServiceLogs.Add(serviceLog);

        unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result > 0)
        {
            if (request.ApplyChangesOnWialon)
            {
                //ExcuteRegistredTasks Here
            }
            return await Result<int>.SuccessAsync(unit.Id);
        }
        else
            return await Result<int>.FailureAsync("TransferTrackingUnit Faild!");

    }
}

