using CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Import;

public class ImportCustomersCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => CustomerCacheKey.Tags;
    public ImportCustomersCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateCustomersTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportCustomersCommandHandler :
             IRequestHandler<CreateCustomersTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportCustomersCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportCustomersCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly CustomerDto _dto = new() { Account = string.Empty, Name = string.Empty };
    //private readonly IMapper _mapper;
    //public ImportCustomersCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportCustomersCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportCustomersCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly CustomerDto _dto = new();
    public ImportCustomersCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportCustomersCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportCustomersCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, CustomerDto, object?>>
            {
                { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.ParentId)], (row, item) => item.ParentId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.ParentId)]].ToString(), out int result) == true ? result : null) },
                { _localizer[_dto.GetMemberDescription(x=>x.Name)], (row, item) => item.Name = row[_localizer[_dto.GetMemberDescription(x=>x.Name)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.Account)], (row, item) => item.Account = row[_localizer[_dto.GetMemberDescription(x=>x.Account)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.UserName)], (row, item) => item.UserName = row[_localizer[_dto.GetMemberDescription(x=>x.UserName)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.BillingPlan)], (row, item) => item.BillingPlan = (BillingPlan)Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x=>x.BillingPlan)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.IsTaxable)], (row, item) => item.IsTaxable =Convert.ToBoolean(row[_localizer[_dto.GetMemberDescription(x=>x.IsTaxable)]]) },
                { _localizer[_dto.GetMemberDescription(x=>x.IsRenewable)], (row, item) => item.IsRenewable =Convert.ToBoolean(row[_localizer[_dto.GetMemberDescription(x=>x.IsRenewable)]]) },
                { _localizer[_dto.GetMemberDescription(x=>x.WUserId)], (row, item) => item.WUserId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.WUserId)]].ToString(), out int result) == true ? result : null) },
                { _localizer[_dto.GetMemberDescription(x=>x.WUnitGroupId)], (row, item) => item.WUnitGroupId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.WUnitGroupId)]].ToString(), out int result) == true ? result : null) },
          { _localizer[_dto.GetMemberDescription(x=>x.Address)], (row, item) => item.Address = row[_localizer[_dto.GetMemberDescription(x=>x.Address)]].ToString() },
           { _localizer[_dto.GetMemberDescription(x=>x.Mobile1)], (row, item) => item.Mobile1 = row[_localizer[_dto.GetMemberDescription(x=>x.Mobile1)]].ToString() },
            { _localizer[_dto.GetMemberDescription(x=>x.Mobile2)], (row, item) => item.Mobile2 = row[_localizer[_dto.GetMemberDescription(x=>x.Mobile2)]].ToString() },
             { _localizer[_dto.GetMemberDescription(x=>x.Email)], (row, item) => item.Email = row[_localizer[_dto.GetMemberDescription(x=>x.Email)]].ToString() },
             { _localizer[_dto.GetMemberDescription(x=>x.IsAvaliable)], (row, item) => item.IsAvaliable =Convert.ToBoolean(row[_localizer[_dto.GetMemberDescription(x=>x.IsAvaliable)]]) },
              { _localizer[_dto.GetMemberDescription(x=>x.OldId)], (row, item) => item.OldId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.OldId)]].ToString(), out int result) == true ? result : null) }

            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.Customers.AnyAsync(x => x.Name == dto.Name, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<Customer>(dto);

                    var item = Mapper.FromDto(dto);

                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.Customers.AddAsync(item, cancellationToken);
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result.Data.Count());
        }
        else
        {
            return await Result<int>.FailureAsync(result.Errors);
        }
    }
    public async Task<Result<byte[]>> Handle(CreateCustomersTemplateCommand request, CancellationToken cancellationToken)
    {
        var fields = new string[] {
                   _localizer[_dto.GetMemberDescription(x=>x.Id)],
                   _localizer[_dto.GetMemberDescription(x=>x.ParentId)],
                   _localizer[_dto.GetMemberDescription(x=>x.Name)],
                   _localizer[_dto.GetMemberDescription(x=>x.Account)],
                   _localizer[_dto.GetMemberDescription(x=>x.UserName)],
                   _localizer[_dto.GetMemberDescription(x=>x.BillingPlan)],
                   _localizer[_dto.GetMemberDescription(x=>x.IsTaxable)],
                   _localizer[_dto.GetMemberDescription(x=>x.IsRenewable)],
                   _localizer[_dto.GetMemberDescription(x=>x.WUserId)],
                   _localizer[_dto.GetMemberDescription(x=>x.WUnitGroupId)],
                   _localizer[_dto.GetMemberDescription(x=>x.Address)],
                   _localizer[_dto.GetMemberDescription(x=>x.Mobile1)],
                   _localizer[_dto.GetMemberDescription(x=>x.Mobile2)],
                   _localizer[_dto.GetMemberDescription(x=>x.Email)],
                   _localizer[_dto.GetMemberDescription(x=>x.IsAvaliable)],
                   _localizer[_dto.GetMemberDescription(x=>x.OldId)],

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }

}


