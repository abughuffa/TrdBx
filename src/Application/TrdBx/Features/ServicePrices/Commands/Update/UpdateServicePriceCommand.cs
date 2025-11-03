using CleanArchitecture.Blazor.Application.Features.ServicePrices.Caching;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Commands.Update;

public class UpdateServicePriceCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Desc")]
    public string? Desc { get; set; }
    [Description("Price")]
    public decimal Price { get; set; }



    public string CacheKey => ServicePriceCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => ServicePriceCacheKey.Tags;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateServicePriceCommand, ServicePrice>(MemberList.None);
            CreateMap<ServicePriceDto, UpdateServicePriceCommand>(MemberList.None);
        }
    }

}

public class UpdateServicePriceCommandHandler : IRequestHandler<UpdateServicePriceCommand, Result<int>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public UpdateServicePriceCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(UpdateServicePriceCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await db.ServicePrices.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("ServicePrice not found");
        _mapper.Map(request, item);
        // raise a update domain event
        item.AddDomainEvent(new ServicePriceUpdatedEvent(item));
        await db.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

