
#nullable enable
#nullable disable warnings
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Create;

public class CreateCusPriceCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "CustomerId")]
    public int CustomerId { get; set; }
    [Display(Name = "TrackingUnitModelId")]
    public int TrackingUnitModelId { get; set; }
    [Display(Name = "Host")]
    public decimal Host { get; set; } = 0.0m;
    [Display(Name = "Gprs")]
    public decimal Gprs { get; set; } = 0.0m;
    [Display(Name = "Price")]
    public decimal Price { get; set; } = 0.0m;

    public string CacheKey => CusPriceCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => CusPriceCacheKey.Tags;

}

public class CreateCusPriceCommandHandler : IRequestHandler<CreateCusPriceCommand, Result<int>>
{

        private readonly IObjectMapper _objectMapper;
        private readonly IApplicationDbContextFactory _dbContextFactory;
        public CreateCusPriceCommandHandler(
            IObjectMapper objectMapper,
            IApplicationDbContextFactory dbContextFactory)
        {
            _objectMapper = objectMapper;
            _dbContextFactory = dbContextFactory;
        }

    public async ValueTask<Result<int>> Handle(CreateCusPriceCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = _objectMapper.Map<CusPrice>(request);
        // raise a create domain event
        item.AddDomainEvent(new CusPriceCreatedEvent(item));
        context.CusPrices.Add(item);
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

