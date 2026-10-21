using System.Text.RegularExpressions;
using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
// using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Create;

public class CreateTrackedAssetCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "TrackedAssetCode")]
    public string? TrackedAssetCode { get; set; }
    [Display(Name = "VinSerNo")]
    public string? VinSerNo { get; set; }
    [Display(Name = "PlateNo")]
    public string? PlateNo { get; set; }
    [Display(Name = "TrackedAssetDesc")]
    public string? TrackedAssetDesc { get; set; }

    public string CacheKey => TrackedAssetCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => TrackedAssetCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<CreateTrackedAssetCommand, TrackedAsset>(MemberList.None);
    //    }
    //}
}

public class CreateTrackedAssetCommandHandler : SerialForSharedLogic, IRequestHandler<CreateTrackedAssetCommand, Result<int>>
{

          private readonly IObjectMapper _objectMapper;
        private readonly IApplicationDbContextFactory _dbContextFactory;
        public CreateTrackedAssetCommandHandler(
            IObjectMapper objectMapper,
            IApplicationDbContextFactory dbContextFactory)
        {
            _objectMapper = objectMapper;
            _dbContextFactory = dbContextFactory;
        }

    public async ValueTask<Result<int>> Handle(CreateTrackedAssetCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var item = _mapper.Map<TrackedAsset>(request);

             await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = _objectMapper.Map<TrackedAsset>(request);


        item.TrackedAssetNo = await GenTrackedAssetSerialNo(context);
        item.TrackedAssetCode = request.TrackedAssetCode ?? request.PlateNo ?? request.VinSerNo ?? "غير محدد";
        // raise a create domain event


        try
        {
            item.AddDomainEvent(new TrackedAssetCreatedEvent(item));
            context.TrackedAssets.Add(item);

            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        catch(Exception EX)
        {
            await context.SaveChangesAsync(cancellationToken);
            return await Result<int>.FailureAsync(EX.Message);
        }



    }


    private async Task<string> GenTrackedAssetSerialNo(IApplicationDbContext cnx)
    {
        //var now = date is null ? DateOnly.FromDateTime(DateTime.Now) : date;
        var prefix = $"{DateTime.Now:yyyyMM}-";
        var sequenceNumber = 1;
        var serialNo = string.Empty;
        var lastTrackedAsset = await cnx.TrackedAssets.Where(i => i.TrackedAssetNo.StartsWith(prefix)).AsNoTracking().OrderByDescending(i => i.TrackedAssetNo).FirstOrDefaultAsync();
                    
        if (lastTrackedAsset != null)
           {
              var match = Regex.Match(lastTrackedAsset.TrackedAssetNo, @$"^{prefix}(\d+)$");
                  if (match.Success && int.TryParse(match.Groups[1].Value, out int lastSequence))
                        {
                            sequenceNumber = lastSequence + 1;
                        }
           }
        serialNo = $"{prefix}{sequenceNumber:D3}";

        return serialNo;
        }

        }



