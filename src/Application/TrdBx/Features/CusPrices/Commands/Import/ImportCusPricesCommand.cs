using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Import;

public class ImportCusPricesCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => CusPriceCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => CusPriceCacheKey.Tags;
    public ImportCusPricesCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateCusPricesTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportCusPricesCommandHandler :
             IRequestHandler<CreateCusPricesTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportCusPricesCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportCusPricesCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly CusPriceDto _dto = new();
    //private readonly IMapper _mapper;
    //public ImportCusPricesCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportCusPricesCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportCusPricesCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly CusPriceDto _dto = new();
    public ImportCusPricesCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportCusPricesCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportCusPricesCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, CusPriceDto, object?>>
            {
               { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.CustomerId)], (row, item) => item.CustomerId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.CustomerId)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.TrackingUnitModelId)], (row, item) => item.TrackingUnitModelId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitModelId)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.Host)], (row, item) => item.Host = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Host)]].ToString())},
               { _localizer[_dto.GetMemberDescription(x=>x.Gprs)], (row, item) => item.Gprs = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Gprs)]].ToString())},
               { _localizer[_dto.GetMemberDescription(x=>x.Price)], (row, item) => item.Price = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Price)]].ToString())},

            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.CusPrices.AnyAsync(x => x.CustomerId == dto.CustomerId && x.TrackingUnitModelId == dto.TrackingUnitModelId, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<CusPrice>(dto);

                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    await _context.CusPrices.AddAsync(item, cancellationToken);
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
    public async Task<Result<byte[]>> Handle(CreateCusPricesTemplateCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement ImportCusPricesCommandHandler method 
        var fields = new string[] {
        // TODO: Define the fields that should be generate in the template, for example:
        _localizer[_dto.GetMemberDescription(x=>x.Id)],
        _localizer[_dto.GetMemberDescription(x=>x.CustomerId)],
        _localizer[_dto.GetMemberDescription(x=>x.TrackingUnitModelId)],
        _localizer[_dto.GetMemberDescription(x=>x.Gprs)],
        _localizer[_dto.GetMemberDescription(x=>x.Host)],
        _localizer[_dto.GetMemberDescription(x=>x.Price)]};

        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);
    }
}

