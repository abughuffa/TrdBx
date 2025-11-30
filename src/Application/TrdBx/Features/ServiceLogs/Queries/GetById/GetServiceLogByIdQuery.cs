using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Queries.GetById;

public class GetServiceLogByIdQuery : ICacheableRequest<Result<ServiceLogDto>>
{
   public required int Id { get; set; }
   public string CacheKey => ServiceLogCacheKey.GetByIdCacheKey($"{Id}");
    public IEnumerable<string> Tags => ServiceLogCacheKey.Tags;
}

public class GetServiceLogByIdQueryHandler :
     IRequestHandler<GetServiceLogByIdQuery, Result<ServiceLogDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetServiceLogByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetServiceLogByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<ServiceLogDto>> Handle(GetServiceLogByIdQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.ServiceLogs.ApplySpecification(new ServiceLogByIdSpecification(request.Id))
        //                                        .ProjectTo<ServiceLogDto>(_mapper.ConfigurationProvider)
        //                                        .FirstAsync(cancellationToken) ?? throw new NotFoundException($"ServiceLog with id: [{request.Id}] not found.");
        //return await Result<ServiceLogDto>.SuccessAsync(data);

        var data = await _context.ServiceLogs.ApplySpecification(new ServiceLogByIdSpecification(request.Id))
                                  .ProjectTo()
                                  .FirstAsync(cancellationToken) ?? throw new NotFoundException($"ServiceLog with id: [{request.Id}] not found.");
        return await Result<ServiceLogDto>.SuccessAsync(data);

    }
}
