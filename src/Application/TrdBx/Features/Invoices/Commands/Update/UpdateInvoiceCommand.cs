//using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
//using CleanArchitecture.Blazor.Application.Features.Invoices.Mappers;

//namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Update;

//public class UpdateInvoiceCommand : ICacheInvalidatorRequest<Result<int>>
//{
//    [Description("Id")]
//    public int Id { get; set; }
//    public DateOnly InvDate { get; set; }
//    [Description("DueDate")]
//    public DateOnly DueDate { get; set; }


//    public string CacheKey => InvoiceCacheKey.GetAllCacheKey;
//     public IEnumerable<string> Tags => InvoiceCacheKey.Tags;

//    //private class Mapping : Profile
//    //{
//    //    public Mapping()
//    //    {
//    //        CreateMap<UpdateInvoiceCommand, Invoice>(MemberList.None);
//    //        CreateMap<InvoiceDto, UpdateInvoiceCommand>(MemberList.None);
//    //    }
//    //}

//}

//public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Result<int>>
//{
//    //private readonly IApplicationDbContextFactory _dbContextFactory;
//    //private readonly IMapper _mapper;
//    //public UpdateInvoiceCommandHandler(
//    //    IApplicationDbContextFactory dbContextFactory,
//    //    IMapper mapper
//    //)
//    //{
//    //    _dbContextFactory = dbContextFactory;
//    //    _mapper = mapper;
//    //}

//    private readonly IApplicationDbContext _context;
//    public UpdateInvoiceCommandHandler(
//        IApplicationDbContext context
//    )
//    {
//        _context = context;
//    }
//    public async Task<Result<int>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
//    {
//        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
//        var item = await _context.Invoices.FindAsync(request.Id, cancellationToken);
//        if (item == null) return await Result<int>.FailureAsync("Invoice not found");

//        //item = _mapper.Map(request, item);

//        Mapper.ApplyChangesFrom(request,item);
//        // raise a update domain event
//        item.AddDomainEvent(new InvoiceUpdatedEvent(item));
//        await _context.SaveChangesAsync(cancellationToken);
//        return await Result<int>.SuccessAsync(item.Id);
//    }
//}

