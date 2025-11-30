using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Create;

public class CreateTrackedAssetCommand : ICacheInvalidatorRequest<Result<int>>
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

    public string CacheKey => TrackedAssetCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackedAssetCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<CreateTrackedAssetCommand, TrackedAsset>(MemberList.None);
    //    }
    //}
}

public class CreateTrackedAssetCommandHandler : SerialForSharedLogic, IRequestHandler<CreateTrackedAssetCommand, Result<int>>
{

    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public CreateTrackedAssetCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}


    private readonly IApplicationDbContext _context;
    public CreateTrackedAssetCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateTrackedAssetCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var item = _mapper.Map<TrackedAsset>(request);

        var item = Mapper.FromCreateCommand(request);
        item.TrackedAssetNo = GenSerialNo(_context, "TrackedAsset", null).Result;
        item.TrackedAssetCode = request.TrackedAssetCode ?? request.PlateNo ?? request.VinSerNo ?? "غير محدد";
        // raise a create domain event
        item.AddDomainEvent(new TrackedAssetCreatedEvent(item));
        _context.TrackedAssets.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);


    }


}



