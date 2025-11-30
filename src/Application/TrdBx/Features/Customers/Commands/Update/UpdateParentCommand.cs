using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Update;

public class UpdateParentCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("Name")]
    public string? Name { get; set; }

    [Description("Account")]
    public string? Account { get; set; }

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
    //        CreateMap<UpdateParentCommand, Customer>(MemberList.None);
    //        CreateMap<CustomerDto, UpdateParentCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateParentCommandHandler : IRequestHandler<UpdateParentCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public UpdateParentCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public UpdateParentCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(UpdateParentCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.Customers.Include(p => p.Childs).FirstAsync(p => p.Id == request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("Customer not found");

        foreach (var c in item.Childs)
        {
            c.Account = item.Account;
            c.IsTaxable = item.IsTaxable;
            c.IsRenewable = item.IsRenewable;
            //c.AddDomainEvent(new CustomerUpdatedEvent(c));
        }

        Mapper.ApplyChangesFrom(request, item);

        //_mapper.Map(request, item);
        // raise a update domain event
        item.AddDomainEvent(new CustomerUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

