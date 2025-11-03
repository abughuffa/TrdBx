using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Create;

public class CreateCusPriceCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("CustomerId")]
    public int CustomerId { get; set; }
    [Description("TrackingUnitModelId")]
    public int TrackingUnitModelId { get; set; }
    [Description("Host")]
    public decimal Host { get; set; } = 0.0m;
    [Description("Gprs")]
    public decimal Gprs { get; set; } = 0.0m;
    [Description("Price")]
    public decimal Price { get; set; } = 0.0m;

    public string CacheKey => CusPriceCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => CusPriceCacheKey.Tags;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateCusPriceCommand, CusPrice>(MemberList.None);
        }

    }
}

public class CreateCusPriceCommandHandler : IRequestHandler<CreateCusPriceCommand, Result<int>>
{

    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public CreateCusPriceCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(CreateCusPriceCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = _mapper.Map<CusPrice>(request);
        // raise a create domain event
        item.AddDomainEvent(new CusPriceCreatedEvent(item));
        db.CusPrices.Add(item);
        await db.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

