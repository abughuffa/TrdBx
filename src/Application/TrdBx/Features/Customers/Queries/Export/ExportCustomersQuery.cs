
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.Export;

public class ExportCustomersQuery : CustomerAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
    public CustomerAdvancedSpecification Specification => new CustomerAdvancedSpecification(this);
    public IEnumerable<string>? Tags => CustomerCacheKey.Tags;
    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword}, {OrderBy}, {SortDirection}";
    }
    public string CacheKey => CustomerCacheKey.GetExportCacheKey($"{this}");

}


public class ExportCustomersQueryHandler :
         IRequestHandler<ExportCustomersQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportCustomersQueryHandler> _localizer;
    //private readonly CustomerDto _dto = new() { Account = string.Empty, Name = string.Empty };
    //public ExportCustomersQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportCustomersQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

      private readonly TypeAdapterConfig _typeAdapterConfig;
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportCustomersQueryHandler> _localizer;
    private readonly CustomerDto _dto = new();
        public ExportCustomersQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory,
            IExcelService excelService,
            IStringLocalizer<ExportCustomersQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(ExportCustomersQuery request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.Customers.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await context.Customers.ApplySpecification(request.Specification)
               .OrderBy($"{request.OrderBy} {request.SortDirection}")
               .ProjectToType<CustomerDto>(_typeAdapterConfig)
               .AsNoTracking()
               .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<CustomerDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.ParentId)],item => item.ParentId},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Name)],item => item.Name},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Account)],item => item.Account},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.UserName)],item => item.UserName},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.BillingPlan)],item => item.BillingPlan},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.IsTaxable)],item => item.IsTaxable},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.IsRenewable)],item => item.IsRenewable},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.WUserId)],item => item.WUserId},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.WUnitGroupId)],item => item.WUnitGroupId},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Address)],item => item.Address},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Mobile1)],item => item.Mobile1},
                {_localizer[_dto.GetMemberDisplayName(x=>x.Mobile2)],item => item.Mobile1},
                {_localizer[_dto.GetMemberDisplayName(x=>x.Email)],item => item.Email},
                 {_localizer[_dto.GetMemberDisplayName(x=>x.IsAvailable)],item => item.IsAvailable},
                 {_localizer[_dto.GetMemberDisplayName(x=>x.OldId)],item => item.OldId},


            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);




    

    }
}
