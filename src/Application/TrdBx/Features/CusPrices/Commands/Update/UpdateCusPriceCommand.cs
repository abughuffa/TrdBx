using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Update;

public class UpdateCusPriceCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("Host")]
    public decimal Host { get; set; }
    [Description("Gprs")]
    public decimal Gprs { get; set; }
    [Description("Price")]
    public decimal Price { get; set; }


    public string CacheKey => CusPriceCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => CusPriceCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateCusPriceCommand, CusPrice>(MemberList.None);
    //        CreateMap<CusPriceDto, UpdateCusPriceCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateCusPriceCommandHandler : IRequestHandler<UpdateCusPriceCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public UpdateCusPriceCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext db;
    public UpdateCusPriceCommandHandler(
        IApplicationDbContext dbContext
    )
    {
        db = dbContext;
    }

    public async Task<Result<int>> Handle(UpdateCusPriceCommand request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await db.CusPrices.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("CusPrice not found");


        //_mapper.Map(request, item);

        Mapper.ApplyChangesFrom(request, item);
        // raise a update domain event
        item.AddDomainEvent(new CusPriceUpdatedEvent(item));
        await db.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}

