using CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Specifications;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.Caching;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.Specifications;
// using CleanArchitecture.Blazor.Application.Features.SmsMessages.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Queries.GetById;

public class GetSmsMessageByIdQuery : ICacheableRequest<Result<SmsMessageDto>>
{
   public required int Id { get; set; }
   public string CacheKey => SmsMessageCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string> Tags => SmsMessageCacheKey.Tags;
}

public class GetSmsMessageByIdQueryHandler :
     IRequestHandler<GetSmsMessageByIdQuery, Result<SmsMessageDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetSmsMessageByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetSmsMessageByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }    public async ValueTask<Result<SmsMessageDto>> Handle(GetSmsMessageByIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.SmsMessages.ApplySpecification(new SmsMessageByIdSpecification(request.Id))
                                .ProjectToType<SmsMessageDto>(_typeAdapterConfig)
                                  .FirstAsync(cancellationToken) ?? throw new NotFoundException($"SmsMessage with id: [{request.Id}] not found.");
        return await Result<SmsMessageDto>.SuccessAsync(data);

    }
}
