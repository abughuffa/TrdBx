

using CleanArchitecture.Blazor.Application.Features.POIs.Caching;
using CleanArchitecture.Blazor.Application.Features.POIs.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.POIs.Commands.Update;

public class UpdatePOICommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = string.Empty;
    [Description("Latitude")]
    public double Latitude { get; set; } = 0.0;
    [Description("Longitude")]
    public double Longitude { get; set; } = 0.0;

    public string CacheKey => POICacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => POICacheKey.Tags;

}

public class UpdatePOICommandHandler : IRequestHandler<UpdatePOICommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public UpdatePOICommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(UpdatePOICommand request, CancellationToken cancellationToken)
    {

       var item = await _context.POIs.FindAsync(request.Id, cancellationToken);
       if (item == null)
       {
           return await Result<int>.FailureAsync($"POI with id: [{request.Id}] not found.");
       }
       POIMapper.ApplyChangesFrom(request, item);
	    // raise a update domain event
	   item.AddDomainEvent(new POIUpdatedEvent(item));
       await _context.SaveChangesAsync(cancellationToken);
       return await Result<int>.SuccessAsync(item.Id);
    }
}

