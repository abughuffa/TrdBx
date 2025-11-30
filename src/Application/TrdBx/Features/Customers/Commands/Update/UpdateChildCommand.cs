using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Update;

public class UpdateChildCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("ParentId")]
    public int ParentId { get; set; }

    [Description("Name")]
    public string? Name { get; set; }

    [Description("UserName")]
    public string? UserName { get; set; }

    [Description("IsTaxable")]
    public bool IsTaxable { get; set; }
    [Description("IsRenewable")]
    public bool IsRenewable { get; set; }

    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => CustomerCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateChildCommand, Customer>(MemberList.None);
    //        CreateMap<CustomerDto, UpdateChildCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateChildCommandHandler : IRequestHandler<UpdateChildCommand, Result<int>>
{

    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public UpdateChildCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public UpdateChildCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(UpdateChildCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.Customers.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("Customer not found");

        Mapper.ApplyChangesFrom(request, item);

        //_mapper.Map(request, item);
        // raise a update domain event
        item.AddDomainEvent(new CustomerUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

