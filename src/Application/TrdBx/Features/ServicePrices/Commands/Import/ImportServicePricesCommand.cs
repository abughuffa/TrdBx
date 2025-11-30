using CleanArchitecture.Blazor.Application.Features.ServicePrices.Caching;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Mappers;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Commands.Import;

public class ImportServicePricesCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => ServicePriceCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => ServicePriceCacheKey.Tags;
    public ImportServicePricesCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateServicePricesTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportServicePricesCommandHandler :
             IRequestHandler<CreateServicePricesTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportServicePricesCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportServicePricesCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly ServicePriceDto _dto = new();
    //private readonly IMapper _mapper;
    //public ImportServicePricesCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportServicePricesCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportServicePricesCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly ServicePriceDto _dto = new();
    public ImportServicePricesCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportServicePricesCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportServicePricesCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ServicePriceDto, object?>>
            {
               { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },

{ _localizer[_dto.GetMemberDescription(x=>x.Desc)], (row, item) => item.Desc = row[_localizer[_dto.GetMemberDescription(x=>x.Desc)]].ToString() },
{ _localizer[_dto.GetMemberDescription(x=>x.ServiceTask)], (row, item) => item.ServiceTask = (ServiceTask)Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x=>x.ServiceTask)]].ToString()) },
  { _localizer[_dto.GetMemberDescription(x=>x.Price)], (row, item) => item.Price = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Price)]].ToString())},

            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.ServicePrices.AnyAsync(x => x.ServiceTask == dto.ServiceTask, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<ServicePrice>(dto);
                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.ServicePrices.AddAsync(item, cancellationToken);
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
    public async Task<Result<byte[]>> Handle(CreateServicePricesTemplateCommand request, CancellationToken cancellationToken)
    {

        var fields = new string[] {
                                    _localizer[_dto.GetMemberDescription(x=>x.Id)],
_localizer[_dto.GetMemberDescription(x=>x.ServiceTask)],
_localizer[_dto.GetMemberDescription(x=>x.Desc)],
_localizer[_dto.GetMemberDescription(x=>x.Price)]

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}



