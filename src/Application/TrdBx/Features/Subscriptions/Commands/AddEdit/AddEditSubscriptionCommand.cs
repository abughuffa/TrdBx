using CleanArchitecture.Blazor.Application.Features.Subscriptions.Caching;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.Mappers;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Commands.AddEdit;

public class AddEditSubscriptionCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("TrackingUnitId")]
    public int TrackingUnitId { get; set; }
    [Description("LastPaidFees")]
    public SubPackageFees LastPaidFees { get; set; }
    [Description("Desc")]
    public required string Desc { get; set; }
    [Description("SubPackageFees")]
    public SubPackageFees SubPackageFees { get; set; }
    [Description("SsDate")]
    public DateOnly SsDate { get; set; }
    [Description("SeDate")]
    public DateOnly SeDate { get; set; }




    public string CacheKey => SubscriptionCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => SubscriptionCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<SubscriptionDto, AddEditSubscriptionCommand>(MemberList.None);
    //        CreateMap<AddEditSubscriptionCommand, SubscriptionDto>(MemberList.None);
    //    }
    //}

}

public class AddEditSubscriptionCommandHandler : IRequestHandler<AddEditSubscriptionCommand, Result<int>>
{
    //private readonly IMapper _mapper;
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public AddEditSubscriptionCommandHandler(
    //    IMapper mapper,
    //    IApplicationDbContextFactory dbContextFactory)
    //{
    //    _mapper = mapper;
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public AddEditSubscriptionCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(AddEditSubscriptionCommand request, CancellationToken cancellationToken)
    {
       // await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        if (request.Id > 0)
        {
            var item = await _context.Subscriptions.FindAsync(request.Id, cancellationToken);
            if (item == null)
            {
                return await Result<int>.FailureAsync($"Subscription with id: [{request.Id}] not found.");
            }
            //item = _mapper.Map(request, item);
            Mapper.ApplyChangesFrom(request, item);
            // raise a update domain event
            item.AddDomainEvent(new SubscriptionUpdatedEvent(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            //var item = _mapper.Map<Subscription>(request);
            var item = Mapper.FromEditCommand(request);
            // raise a create domain event
            item.AddDomainEvent(new SubscriptionCreatedEvent(item));
            _context.Subscriptions.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }



    }
}

