using CleanArchitecture.Blazor.Application.Features.ServicePrices.Caching;
// using CleanArchitecture.Blazor.Application.Features.ServicePrices.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Commands.Update;

public class UpdateServicePriceCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "Description")]
    public string? Description { get; set; }
    [Display(Name = "Price")]
    public decimal Price { get; set; }



    public string CacheKey => ServicePriceCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => ServicePriceCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateServicePriceCommand, ServicePrice>(MemberList.None);
    //        CreateMap<ServicePriceDto, UpdateServicePriceCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateServicePriceCommandHandler : IRequestHandler<UpdateServicePriceCommand, Result<int>>
{
          private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IObjectMapper _objectMapper;
    public UpdateServicePriceCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _objectMapper = objectMapper;
    }
    public async ValueTask<Result<int>> Handle(UpdateServicePriceCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);



        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await context.ServicePrices.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("ServicePrice not found");


        item = _objectMapper.Map(request, item);
        // raise a update domain event
        item.AddDomainEvent(new ServicePriceUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

