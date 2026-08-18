// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.Mappers;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.Specifications;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.Queries.Export;

public class ExportLibyanaSimCardsQuery : LibyanaSimCardAdvancedFilter, IRequest<Result<byte[]>>
{
    public LibyanaSimCardAdvancedSpecification Specification => new(this);

}

public class ExportLibyanaSimCardsQueryHandler :
         IRequestHandler<ExportLibyanaSimCardsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportLibyanaSimCardsQueryHandler> _localizer;
    //private readonly LibyanaSimCardDto _dto = new();
    //public ExportLibyanaSimCardsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportLibyanaSimCardsQueryHandler> localizer
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
    private readonly IStringLocalizer<ExportLibyanaSimCardsQueryHandler> _localizer;
    private readonly LibyanaSimCardDto _dto = new();
        public ExportLibyanaSimCardsQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory,
            IExcelService excelService,
            IStringLocalizer<ExportLibyanaSimCardsQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(ExportLibyanaSimCardsQuery request, CancellationToken cancellationToken)
    {
        byte[] result;
        List<LibyanaSimCardDto> data;
        Dictionary<string, Func<LibyanaSimCardDto, object?>> mappers;

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        mappers = new Dictionary<string, Func<LibyanaSimCardDto, object?>>
                {
                       {_localizer[_dto.GetMemberDisplayName(x=>x.SimCardNo)],item => item.SimCardNo},
                            {_localizer[_dto.GetMemberDisplayName(x=>x.SimCardStatus)],item => item.SimCardStatus},
                            {_localizer[_dto.GetMemberDisplayName(x=>x.Balance)],item => item.Balance},
                            {_localizer[_dto.GetMemberDisplayName(x=>x.BExDate)],item => item.BExDate},
                            {_localizer[_dto.GetMemberDisplayName(x=>x.JoinDate)],item => item.JoinDate},
                            {_localizer[_dto.GetMemberDisplayName(x=>x.Package)],item => item.Package},
                            {_localizer[_dto.GetMemberDisplayName(x=>x.DExDate)],item => item.DExDate},
                            {_localizer[_dto.GetMemberDisplayName(x=>x.DataOffer)],item => item.DataOffer},
                            {_localizer[_dto.GetMemberDisplayName(x=>x.DOExpired)],item => item.DOExpired}
                };

        switch (request.ListView)
        {
            case LibyanaSimCardListView.All:
                {
                    data = await context.LibyanaSimCards
                                               .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                               .ApplySpecification(request.Specification)
                                                .AsNoTracking()
                                 .ProjectToType<LibyanaSimCardDto>(_typeAdapterConfig)
                         .ToListAsync(cancellationToken);

                    break;

                }
            case LibyanaSimCardListView.SimCardsNotExistOnTrdBx:
                {
                    var tSimCards = await context.TrackingUnits.Select(o => o.SimCard.SimCardNo).AsNoTracking().ToListAsync(cancellationToken);

                    data = await context.LibyanaSimCards.Where(o => !tSimCards.Contains(o.SimCardNo))
                                                           .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                           .ApplySpecification(request.Specification)
                                                           .AsNoTracking()
                           .ProjectToType<LibyanaSimCardDto>(_typeAdapterConfig)
                         .ToListAsync(cancellationToken);

                    break;

                }
            case LibyanaSimCardListView.SimCardsNotExistOnWialon:
                {
                    var wSimCards = await context.WialonUnits.Select(o => o.SimCardNo).AsNoTracking().ToListAsync(cancellationToken);

                    data = await context.LibyanaSimCards.Where(o => !wSimCards.Contains(o.SimCardNo))
                                                            .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                            .ApplySpecification(request.Specification)
                                                           .AsNoTracking()
                                   .ProjectToType<LibyanaSimCardDto>(_typeAdapterConfig)
                         .ToListAsync(cancellationToken);

                    break;

                }
            default:
                {
                    data = new List<LibyanaSimCardDto>();
                    break;
                }
        }

        result = await _excelService.ExportAsync(data, mappers, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);


    }
}
