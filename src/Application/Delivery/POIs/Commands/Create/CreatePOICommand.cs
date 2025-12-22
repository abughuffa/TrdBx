

using CleanArchitecture.Blazor.Application.Features.POIs.Caching;
using CleanArchitecture.Blazor.Application.Features.POIs.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.POIs.Commands.Create;

public class CreatePOICommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = string.Empty;
    [Description("Latitude")]
    public double Latitude { get; set; } = 32.8877;
    [Description("Longitude")]
    public double Longitude { get; set; } = 13.1872;




    public string CacheKey => POICacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => POICacheKey.Tags;
}
    
    public class CreatePOICommandHandler : IRequestHandler<CreatePOICommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        public CreatePOICommandHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<int>> Handle(CreatePOICommand request, CancellationToken cancellationToken)
        {
           var item = POIMapper.FromCreateCommand(request);
           // raise a create domain event
	       item.AddDomainEvent(new POICreatedEvent(item));
           _context.POIs.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  await Result<int>.SuccessAsync(item.Id);
        }
    }

