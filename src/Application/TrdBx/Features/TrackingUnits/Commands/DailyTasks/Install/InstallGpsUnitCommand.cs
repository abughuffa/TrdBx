using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.MarkTrackingUnitAsDamaged;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Install;

public class InstallTrackingUnitCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")] public int Id { get; set; }
    [Description("TsDate")] public DateOnly TsDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Description("SimCardId")] public int SimCardId { get; set; }
    [Description("TrackedAssetId")] public int TrackedAssetId { get; set; }
    [Description("CustomerId")] public int CustomerId { get; set; }
    //[Description("InstallerId")] public string InstallerId { get; set; } = string.Empty;
    [Description("SubPackage")] public SubPackage SubPackage { get; set; } = SubPackage.Active;
    [Description("InsMode")] public InsMode InsMode { get; set; } = InsMode.Advanced;
    [Description("CreateDeservedServices")] public bool CreateDeservedServices { get; set; } = true;
    [Description("ApplyChangesOnWialon")] public bool ApplyChangesOnWialon { get; set; } = true;


    public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitDto, InstallTrackingUnitCommand>(MemberList.None)
    //            //.ForMember(dest => dest.Id, opt => opt.Ignore())
    //            //.ForMember(dest => dest.TsDate, opt => opt.Ignore())
    //            .ForMember(dest => dest.SimCardId, opt => opt.Ignore())
    //            //.ForMember(dest => dest.TrackedAssetId, opt => opt.Ignore())
    //            .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
    //            //.ForMember(dest => dest.InstallerId, opt => opt.Ignore())
    //            //.ForMember(dest => dest.SubPackage, opt => opt.Ignore())
    //            //.ForMember(dest => dest.InsMode, opt => opt.Ignore())
    //            .ForMember(dest => dest.CreateDeservedServices, opt => opt.Ignore())
    //            .ForMember(dest => dest.ApplyChangesOnWialon, opt => opt.Ignore());

    //    }
    //}


}

public class InstallTrackingUnitCommandHandler : SubscriptionSharedLogic, IRequestHandler<InstallTrackingUnitCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<InstallTrackingUnitCommandHandler> _localizer;
    //private readonly IWialonService _wialonService;
    //public InstallTrackingUnitCommandHandler(IApplicationDbContextFactory dbContextFactory,
    //                                     IStringLocalizer<InstallTrackingUnitCommandHandler> localizer,
    //                                     IWialonService wialonService)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _wialonService = wialonService;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<InstallTrackingUnitCommandHandler> _localizer;
    private readonly IWialonService _wialonService;
    public InstallTrackingUnitCommandHandler(IApplicationDbContext context,
                                         IStringLocalizer<InstallTrackingUnitCommandHandler> localizer,
                                         IWialonService wialonService)
    {
        _context = context;
        _localizer = localizer;
        _wialonService = wialonService;
    }
    public async Task<Result<int>> Handle(InstallTrackingUnitCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var unit = await _context.TrackingUnits.Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

        if (!(unit.UStatus == UStatus.New || unit.UStatus == UStatus.Reserved || unit.UStatus == UStatus.Used))
        {
            return await Result<int>.FailureAsync("Tracking Unit status should be New, Reserved or used to procced");
        }

        if (unit.UStatus == UStatus.New || unit.UStatus == UStatus.Reserved)
        {
            unit.Subscriptions = new List<Subscription>();
        }

        var sim = await _context.SimCards.FindAsync(new object[] { request.SimCardId }, cancellationToken) ?? throw new NotFoundException($"SimCard with id: [{request.SimCardId}] not found.");

        var asset = await _context.TrackedAssets.FindAsync(new object[] { request.TrackedAssetId }, cancellationToken) ?? throw new NotFoundException($"TrackedAsset with id: [{request.TrackedAssetId}] not found.");

        var price = await GetCPrice(_context, (int)request.CustomerId, unit.TrackingUnitModelId);

        var serviceNo = await GenSerialNo(_context, "ServiceLog", request.TsDate);

        var serviceLog = new ServiceLog()
        {
            ServiceNo = serviceNo,
            CustomerId = (int)request.CustomerId,
            //InstallerId = request.InstallerId,
            SerDate = request.TsDate,
            IsBilled = false,
            Subscriptions = [],
            WialonTasks = []
        };

        switch (unit.UStatus)
        {
            case UStatus.Used:
                {
                    serviceLog.ServiceTask = ServiceTask.ReInstall;
                    serviceLog.Desc = string.Format("إعادة تركيب الوحدة المستعملة ({0}) في الأصل ({1}).", unit.SNo, asset.TrackedAssetNo);
                    serviceLog.IsDeserved = request.CreateDeservedServices;
                    serviceLog.Amount = await GetSPrice(_context, ServiceTask.ReInstall);

                    unit.CustomerId = request.CustomerId;

                    if (unit.SimCardId != null && sim.Id != unit.SimCardId)
                    {
                        var oldSimCard = _context.SimCards.Where(a => a.Id == (int)unit.SimCardId).First();
                        oldSimCard.SStatus = SStatus.Recovered; //Set as Recovered
                        oldSimCard.AddDomainEvent(new SimCardUpdatedEvent(oldSimCard));
                    }
                    break;
                }
            case UStatus.New:
            case UStatus.Reserved:
                {
                    serviceLog.ServiceTask = ServiceTask.Install;
                    serviceLog.Desc = string.Format("تركيب الوحدة الجديدة ({0}) في الأصل ({1}).", unit.SNo, asset.TrackedAssetNo);
                    serviceLog.IsDeserved = true;

                    serviceLog.Amount = price.Price;

                    unit.WryDate = request.TsDate.AddDays(365);
                    unit.CustomerId = request.CustomerId;
                    break;
                }
        }

        sim.SStatus = SStatus.Installed;
        sim.AddDomainEvent(new SimCardUpdatedEvent(sim));

        asset.IsAvaliable = false;
        asset.AddDomainEvent(new TrackedAssetUpdatedEvent(asset));

        unit.UStatus = UStatus.InstalledInactive;
        unit.UnitName = asset.TrackedAssetCode;
        unit.TrackedAssetId = request.TrackedAssetId;
        unit.SimCardId = request.SimCardId;
        unit.InsMode = request.InsMode;


        switch (request.SubPackage)
        {
            case SubPackage.Active:
                {
                    Activate(unit, serviceLog, request.TsDate, price,true);
                    break;
                }
            case SubPackage.ActiveHosting:
                {
                    ActivateForHosting(unit, serviceLog, request.TsDate, price,true);
                    break;
                }
            case SubPackage.ActiveGprs:
                {
                    ActivateForGprs(unit, serviceLog, request.TsDate, price,true);
                    break;
                }
        }

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

