using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Update;

public class UpdateTrackedAssetCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("TrackedAssetCode")]
    public string? TrackedAssetCode { get; set; }
    [Description("VinSerNo")]
    public string? VinSerNo { get; set; }
    [Description("PlateNo")]
    public string? PlateNo { get; set; }
    [Description("TrackedAssetDesc")]
    public string? TrackedAssetDesc { get; set; }
    [Description("IsAvaliable")]
    public bool IsAvaliable { get; set; }


    public string CacheKey => TrackedAssetCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackedAssetCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateTrackedAssetCommand, TrackedAsset>(MemberList.None);
    //        CreateMap<TrackedAssetDto, UpdateTrackedAssetCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateTrackedAssetCommandHandler : IRequestHandler<UpdateTrackedAssetCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public UpdateTrackedAssetCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}


    private readonly IApplicationDbContext _context;
    public UpdateTrackedAssetCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(UpdateTrackedAssetCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.TrackedAssets.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("TrackedAsset not found");
        //_mapper.Map(request, item);
        Mapper.ApplyChangesFrom(request, item);
        item.TrackedAssetCode = request.TrackedAssetCode != null ? request.TrackedAssetCode : request.PlateNo != null ? request.PlateNo : request.VinSerNo != null ? request.VinSerNo : "غير محدد";
        // raise a update domain event
        item.AddDomainEvent(new TrackedAssetUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

