using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
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


        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IStringLocalizer<ImportServiceLogsCommandHandler> _localizer;
        private readonly IExcelService _excelService;
        private readonly ServiceLogDto _dto = new();
        private readonly IObjectMapper _objectMapper;
        public ImportServiceLogsCommandHandler(
            IApplicationDbContextFactory dbContextFactory,
            IObjectMapper objectMapper,
            IExcelService excelService,
            IStringLocalizer<ImportServiceLogsCommandHandler> localizer)
        {
            _dbContextFactory = dbContextFactory;
            _localizer = localizer;
            _excelService = excelService;
            _objectMapper = objectMapper;
        }
        #nullable disable warnings
    public async ValueTask<Result<int>> Handle(ImportServiceLogsCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ServiceLogDto, object?>>
            {
             //{ _localizer[_dto.GetMemberDisplayName(x=>x.Id)], (row, item) => item.Id = int.Parse(row[0].ToString()) },
             //       { _localizer[_dto.GetMemberDisplayName(x=>x.ServiceNo)], (row, item) => item.ServiceNo = row[1].ToString() },
             //       { _localizer[_dto.GetMemberDisplayName(x=>x.ServiceTask)], (row, item) => item.ServiceTask = (ServiceTask)Convert.ToInt32(row[2].ToString()) },
             //       { _localizer[_dto.GetMemberDisplayName(x=>x.CustomerId)], (row, item) => item.CustomerId = int.Parse(row[3].ToString()) },
             //       { _localizer[_dto.GetMemberDisplayName(x=>x.InstallerId)], (row, item) => item.InstallerId = row[4].ToString() },
             //       { _localizer[_dto.GetMemberDisplayName(x=>x.Desc)], (row, item) => item.Desc = row[5].ToString() },
             //       { _localizer[_dto.GetMemberDisplayName(x=>x.SerDate)], (row, item) => item.SerDate = DateOnly.FromDateTime(DateTime.Parse(row[6].ToString()))},
             //       { _localizer[_dto.GetMemberDisplayName(x=>x.IsDeserved)], (row, item) => item.IsDeserved =Convert.ToBoolean(row[7]) },
             //       { _localizer[_dto.GetMemberDisplayName(x=>x.IsBilled)], (row, item) => item.IsBilled =Convert.ToBoolean(row[8]) },
             //       { _localizer[_dto.GetMemberDisplayName(x=>x.Amount)], (row, item) => item.Amount = decimal.Parse(row[9].ToString())},

                    { _localizer[_dto.GetMemberDisplayName(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.Id)]].ToString()) },
                    { _localizer[_dto.GetMemberDisplayName(x=>x.ServiceNo)], (row, item) => item.ServiceNo = row[_localizer[_dto.GetMemberDisplayName(x=>x.ServiceNo)]].ToString() },
                    { _localizer[_dto.GetMemberDisplayName(x=>x.ServiceTask)], (row, item) => item.ServiceTask = (ServiceTask)Convert.ToInt32(row[_localizer[_dto.GetMemberDisplayName(x=>x.ServiceTask)]].ToString()) },
                    { _localizer[_dto.GetMemberDisplayName(x=>x.CustomerId)], (row, item) => item.CustomerId = int.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.CustomerId)]].ToString()) },
                    //{ _localizer[_dto.GetMemberDisplayName(x=>x.InstallerId)], (row, item) => item.InstallerId = row[_localizer[_dto.GetMemberDisplayName(x=>x.InstallerId)]].ToString() },
                    { _localizer[_dto.GetMemberDisplayName(x=>x.Description)], (row, item) => item.Description = row[_localizer[_dto.GetMemberDisplayName(x=>x.Description)]].ToString() },
                    { _localizer[_dto.GetMemberDisplayName(x=>x.SerDate)], (row, item) => item.SerDate = DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.SerDate)]].ToString()))},
                    { _localizer[_dto.GetMemberDisplayName(x=>x.IsDeserved)], (row, item) => item.IsDeserved =Convert.ToBoolean(row[_localizer[_dto.GetMemberDisplayName(x=>x.IsDeserved)]]) },
                    { _localizer[_dto.GetMemberDisplayName(x=>x.IsBilled)], (row, item) => item.IsBilled =Convert.ToBoolean(row[_localizer[_dto.GetMemberDisplayName(x=>x.IsBilled)]]) },
                    { _localizer[_dto.GetMemberDisplayName(x=>x.Amount)], (row, item) => item.Amount = decimal.Parse(row[_localizer[_dto.GetMemberDisplayName(x=>x.Amount)]].ToString())}
            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await context.ServiceLogs.AnyAsync(x => x.ServiceNo == dto.ServiceNo, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<ServiceLog>(dto);
                    var item = _objectMapper.Map<ServiceLog>(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await context.ServiceLogs.AddAsync(item, cancellationToken);
                }
            }
            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result.Data.Count());
        }
        else
        {
            return await Result<int>.FailureAsync(result.Errors);
        }
    }
    public async ValueTask<Result<byte[]>> Handle(CreateServiceLogsTemplateCommand request, CancellationToken cancellationToken)
    {

        var fields = new string[] {
                                    _localizer[_dto.GetMemberDisplayName(x=>x.Id)],
                                    _localizer[_dto.GetMemberDisplayName(x=>x.ServiceNo)],
                                    _localizer[_dto.GetMemberDisplayName(x=>x.ServiceTask)],
                                    _localizer[_dto.GetMemberDisplayName(x=>x.CustomerId)],
                                    //_localizer[_dto.GetMemberDisplayName(x=>x.InstallerId)],
                                    _localizer[_dto.GetMemberDisplayName(x=>x.Description)],
                                    _localizer[_dto.GetMemberDisplayName(x=>x.SerDate)],
                                    _localizer[_dto.GetMemberDisplayName(x=>x.IsDeserved)],
                                    _localizer[_dto.GetMemberDisplayName(x=>x.IsBilled)],
                                    _localizer[_dto.GetMemberDisplayName(x=>x.Amount)]

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

