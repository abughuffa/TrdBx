using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
    using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Mappers;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Update;

public class UpdateServiceLogCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("CustomerId")]
    public int CustomerId { get; set; }
    [Description("InstallerId")]
    public int InstallerId { get; set; }
    [Description("Desc")]
    public string Desc { get; set; } = string.Empty;

    [Description("IsDeserved")]
    public bool IsDeserved { get; set; } = true;
    [Description("IsBilled")]
    public bool IsBilled { get; set; } = false;
    [Description("Amount")]
    public decimal Amount { get; set; } = 0.0m;


    public string CacheKey => ServiceLogCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => ServiceLogCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateServiceLogCommand, ServiceLog>(MemberList.None);
    //        CreateMap<ServiceLogDto, UpdateServiceLogCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateServiceLogCommandHandler : IRequestHandler<UpdateServiceLogCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public UpdateServiceLogCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public UpdateServiceLogCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(UpdateServiceLogCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.ServiceLogs.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("ServiceLog not found");
        //_mapper.Map(request, item);
        Mapper.ApplyChangesFrom(request, item);
        // raise a update domain event
        item.AddDomainEvent(new ServiceLogUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

