using CleanArchitecture.Blazor.Application.Features.SPackages.Caching;
using CleanArchitecture.Blazor.Application.Features.SPackages.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SPackages.Commands.Import;

public class ImportSPackagesCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => SPackageCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => SPackageCacheKey.Tags;
    public ImportSPackagesCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateSPackagesTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportSPackagesCommandHandler :
             IRequestHandler<CreateSPackagesTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportSPackagesCommand, Result<int>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IStringLocalizer<ImportSPackagesCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly SPackageDto _dto = new();
    private readonly IMapper _mapper;
    public ImportSPackagesCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper,
        IExcelService excelService,
        IStringLocalizer<ImportSPackagesCommandHandler> localizer)
    {
        _dbContextFactory = dbContextFactory;
        _localizer = localizer;
        _excelService = excelService;
        _mapper = mapper;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportSPackagesCommand request, CancellationToken cancellationToken)
    {

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, SPackageDto, object?>>
            {
                { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.Name)], (row, item) => item.Name = row[_localizer[_dto.GetMemberDescription(x=>x.Name)]].ToString() },
                 { _localizer[_dto.GetMemberDescription(x=>x.OldId)], (row, item) => item.OldId = (int.TryParse(row[_localizer[_dto.GetMemberDescription(x=>x.OldId)]].ToString(), out int result) == true ? result : null) }
            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await db.SPackages.AnyAsync(x => x.Name == dto.Name, cancellationToken);
                if (!exists)
                {
                    var item = _mapper.Map<SPackage>(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await db.SPackages.AddAsync(item, cancellationToken);
                }
            }
            await db.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result.Data.Count());
        }
        else
        {
            return await Result<int>.FailureAsync(result.Errors);
        }

    }
    public async Task<Result<byte[]>> Handle(CreateSPackagesTemplateCommand request, CancellationToken cancellationToken)
    {
        var fields = new string[] {
                  _localizer[_dto.GetMemberDescription(x=>x.Name)],
                  _localizer[_dto.GetMemberDescription(x=>x.OldId)],

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

