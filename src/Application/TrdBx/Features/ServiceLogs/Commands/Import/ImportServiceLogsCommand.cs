using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Mappers;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Import;

public class ImportServiceLogsCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => ServiceLogCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => ServiceLogCacheKey.Tags;
    public ImportServiceLogsCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateServiceLogsTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportServiceLogsCommandHandler :
             IRequestHandler<CreateServiceLogsTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportServiceLogsCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportServiceLogsCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly ServiceLogDto _dto = new() { Desc = string.Empty, InstallerId = string.Empty };
    //private readonly IMapper _mapper;
    //public ImportServiceLogsCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportServiceLogsCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportServiceLogsCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly ServiceLogDto _dto = new() { Desc = string.Empty, InstallerId = string.Empty };
    public ImportServiceLogsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportServiceLogsCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportServiceLogsCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ServiceLogDto, object?>>
            {
             //{ _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[0].ToString()) },
             //       { _localizer[_dto.GetMemberDescription(x=>x.ServiceNo)], (row, item) => item.ServiceNo = row[1].ToString() },
             //       { _localizer[_dto.GetMemberDescription(x=>x.ServiceTask)], (row, item) => item.ServiceTask = (ServiceTask)Convert.ToInt32(row[2].ToString()) },
             //       { _localizer[_dto.GetMemberDescription(x=>x.CustomerId)], (row, item) => item.CustomerId = int.Parse(row[3].ToString()) },
             //       { _localizer[_dto.GetMemberDescription(x=>x.InstallerId)], (row, item) => item.InstallerId = row[4].ToString() },
             //       { _localizer[_dto.GetMemberDescription(x=>x.Desc)], (row, item) => item.Desc = row[5].ToString() },
             //       { _localizer[_dto.GetMemberDescription(x=>x.SerDate)], (row, item) => item.SerDate = DateOnly.FromDateTime(DateTime.Parse(row[6].ToString()))},
             //       { _localizer[_dto.GetMemberDescription(x=>x.IsDeserved)], (row, item) => item.IsDeserved =Convert.ToBoolean(row[7]) },
             //       { _localizer[_dto.GetMemberDescription(x=>x.IsBilled)], (row, item) => item.IsBilled =Convert.ToBoolean(row[8]) },
             //       { _localizer[_dto.GetMemberDescription(x=>x.Amount)], (row, item) => item.Amount = decimal.Parse(row[9].ToString())},

                    { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
                    { _localizer[_dto.GetMemberDescription(x=>x.ServiceNo)], (row, item) => item.ServiceNo = row[_localizer[_dto.GetMemberDescription(x=>x.ServiceNo)]].ToString() },
                    { _localizer[_dto.GetMemberDescription(x=>x.ServiceTask)], (row, item) => item.ServiceTask = (ServiceTask)Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x=>x.ServiceTask)]].ToString()) },
                    { _localizer[_dto.GetMemberDescription(x=>x.CustomerId)], (row, item) => item.CustomerId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.CustomerId)]].ToString()) },
                    { _localizer[_dto.GetMemberDescription(x=>x.InstallerId)], (row, item) => item.InstallerId = row[_localizer[_dto.GetMemberDescription(x=>x.InstallerId)]].ToString() },
                    { _localizer[_dto.GetMemberDescription(x=>x.Desc)], (row, item) => item.Desc = row[_localizer[_dto.GetMemberDescription(x=>x.Desc)]].ToString() },
                    { _localizer[_dto.GetMemberDescription(x=>x.SerDate)], (row, item) => item.SerDate = DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.SerDate)]].ToString()))},
                    { _localizer[_dto.GetMemberDescription(x=>x.IsDeserved)], (row, item) => item.IsDeserved =Convert.ToBoolean(row[_localizer[_dto.GetMemberDescription(x=>x.IsDeserved)]]) },
                    { _localizer[_dto.GetMemberDescription(x=>x.IsBilled)], (row, item) => item.IsBilled =Convert.ToBoolean(row[_localizer[_dto.GetMemberDescription(x=>x.IsBilled)]]) },
                    { _localizer[_dto.GetMemberDescription(x=>x.Amount)], (row, item) => item.Amount = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Amount)]].ToString())}
            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.ServiceLogs.AnyAsync(x => x.ServiceNo == dto.ServiceNo, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<ServiceLog>(dto);
                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.ServiceLogs.AddAsync(item, cancellationToken);
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
    public async Task<Result<byte[]>> Handle(CreateServiceLogsTemplateCommand request, CancellationToken cancellationToken)
    {

        var fields = new string[] {
                                    _localizer[_dto.GetMemberDescription(x=>x.Id)],
                                    _localizer[_dto.GetMemberDescription(x=>x.ServiceNo)],
                                    _localizer[_dto.GetMemberDescription(x=>x.ServiceTask)],
                                    _localizer[_dto.GetMemberDescription(x=>x.CustomerId)],
                                    _localizer[_dto.GetMemberDescription(x=>x.InstallerId)],
                                    _localizer[_dto.GetMemberDescription(x=>x.Desc)],
                                    _localizer[_dto.GetMemberDescription(x=>x.SerDate)],
                                    _localizer[_dto.GetMemberDescription(x=>x.IsDeserved)],
                                    _localizer[_dto.GetMemberDescription(x=>x.IsBilled)],
                                    _localizer[_dto.GetMemberDescription(x=>x.Amount)]

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

