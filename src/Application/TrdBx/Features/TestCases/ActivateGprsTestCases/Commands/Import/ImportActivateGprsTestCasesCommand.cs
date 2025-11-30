using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Caching;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.DTOs;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Commands.Import;

public class ImportActivateGprsTestCasesCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => ActivateGprsTestCaseCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => ActivateGprsTestCaseCacheKey.Tags;
    public ImportActivateGprsTestCasesCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateActivateGprsTestCasesTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportActivateGprsTestCasesCommandHandler :
             IRequestHandler<CreateActivateGprsTestCasesTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportActivateGprsTestCasesCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportActivateGprsTestCasesCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly ActivateGprsTestCaseDto _dto = new() { InstallerId = string.Empty };
    //private readonly IMapper _mapper;
    //public ImportActivateGprsTestCasesCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportActivateGprsTestCasesCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportActivateGprsTestCasesCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly ActivateGprsTestCaseDto _dto = new() { InstallerId = string.Empty };
    public ImportActivateGprsTestCasesCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportActivateGprsTestCasesCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportActivateGprsTestCasesCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ActivateGprsTestCaseDto, object?>>
            {
               { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.CaseCode)], (row, item) => item.CaseCode = row[_localizer[_dto.GetMemberDescription(x=>x.CaseCode)]].ToString() != null ? Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x => x.CaseCode)]].ToString()) : null },
               { _localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)], (row, item) => item.TrackingUnitId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.InstallerId)], (row, item) => item.InstallerId = row[_localizer[_dto.GetMemberDescription(x=>x.InstallerId)]].ToString() },
               { _localizer[_dto.GetMemberDescription(x=>x.SNo)], (row, item) => item.SNo = row[_localizer[_dto.GetMemberDescription(x=>x.SNo)]].ToString() },
               { _localizer[_dto.GetMemberDescription(x=>x.TsDate)], (row, item) => item.TsDate = DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.TsDate)]].ToString()))},
        }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.ActivateGprsTestCases.AnyAsync(x => x.Id == dto.Id, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<ActivateGprsTestCase>(dto);


                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.ActivateGprsTestCases.AddAsync(item, cancellationToken);
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
    public async Task<Result<byte[]>> Handle(CreateActivateGprsTestCasesTemplateCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement ImportActivateGprsTestCasesCommandHandler method 
        var fields = new string[] {
                   // TODO: Define the fields that should be generate in the template, for example:
                   _localizer[_dto.GetMemberDescription(x=>x.Id)],
                   _localizer[_dto.GetMemberDescription(x=>x.CaseCode)],
_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)],
_localizer[_dto.GetMemberDescription(x=>x.InstallerId)],
_localizer[_dto.GetMemberDescription(x=>x.SNo)],
_localizer[_dto.GetMemberDescription(x=>x.TsDate)],

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

