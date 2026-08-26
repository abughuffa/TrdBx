using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
// using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Update;

public class UpdateTrackedAssetCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "TrackedAssetCode")]
    public string? TrackedAssetCode { get; set; }
    [Display(Name = "VinSerNo")]
    public string? VinSerNo { get; set; }
    [Display(Name = "PlateNo")]
    public string? PlateNo { get; set; }
    [Display(Name = "TrackedAssetDesc")]
    public string? TrackedAssetDesc { get; set; }
    [Display(Name = "IsAvailable")]
    public bool IsAvailable { get; set; }


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
         private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IObjectMapper _objectMapper;
    public UpdateTrackedAssetCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _objectMapper = objectMapper;
    }

    public async ValueTask<Result<int>> Handle(UpdateTrackedAssetCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

       

        var item = await context.TrackedAssets.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("TrackedAsset not found");


        item = _objectMapper.Map(request, item);
        item.TrackedAssetCode = request.TrackedAssetCode != null ? request.TrackedAssetCode : request.PlateNo != null ? request.PlateNo : request.VinSerNo != null ? request.VinSerNo : "غير محدد";
        // raise a update domain event
        item.AddDomainEvent(new TrackedAssetUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

