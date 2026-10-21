

using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Update;

public class UpdateCusPriceCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Host")]
    public decimal Host { get; set; }
    [Display(Name = "Gprs")]
    public decimal Gprs { get; set; }
    [Display(Name = "Price")]
    public decimal Price { get; set; }


    public string CacheKey => CusPriceCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => CusPriceCacheKey.Tags;


}

public class UpdateCusPriceCommandHandler : IRequestHandler<UpdateCusPriceCommand, Result<int>>
{


      private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IObjectMapper _objectMapper;
    public UpdateCusPriceCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _objectMapper = objectMapper;
    }

    public async ValueTask<Result<int>> Handle(UpdateCusPriceCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await context.CusPrices.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("CusPrice not found");


        //_mapper.Map(request, item);

        item = _objectMapper.Map(request, item);

        // raise a update domain event
        item.AddDomainEvent(new CusPriceUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}

