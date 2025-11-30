// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Mappers;
using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Queries.Pagination;






public class LibyanaSimCardsWithPaginationQuery : LibyanaSimCardAdvancedFilter, ICacheableRequest<PaginatedData<LibyanaSimCardDto>?>
{
    public IEnumerable<string>? Tags => LibyanaSimCardCacheKey.Tags;
    public LibyanaSimCardAdvancedSpecification Specification => new(this);
    public string CacheKey => LibyanaSimCardCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }
}

public class LibyanaSimCardsWithPaginationQueryHandler :
         IRequestHandler<LibyanaSimCardsWithPaginationQuery, PaginatedData<LibyanaSimCardDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public LibyanaSimCardsWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public LibyanaSimCardsWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<PaginatedData<LibyanaSimCardDto>> Handle(LibyanaSimCardsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        switch (request.ListView)
        {
            case LibyanaSimCardListView.All:
                {
                    //var data = await _context.LibyanaSimCards.OrderBy($"{request.OrderBy} {request.SortDirection}")
                    //                               .ProjectToPaginatedDataAsync<LibyanaSimCard, LibyanaSimCardDto>(request.Specification,
                    //                                request.PageNumber,
                    //                                request.PageSize,
                    //                                _mapper.ConfigurationProvider,
                    //                                cancellationToken);
                    var data = await _context.LibyanaSimCards.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                   .ProjectToPaginatedDataAsync(request.Specification, request.PageNumber, request.PageSize, Mapper.ToDto, cancellationToken);
                    return data;
                }
            case LibyanaSimCardListView.SimCardsNotExistOnTrdBx:
                {
                    var tSimCards = await _context.TrackingUnits.Select(o => o.SimCard.SimCardNo).ToListAsync(cancellationToken);

                    //var data = await _context.LibyanaSimCards.Where(o => !tSimCards.Contains(o.SimCardNo))
                    //                            .OrderBy($"{request.OrderBy} {request.SortDirection}")
                    //                              .ProjectToPaginatedDataAsync<LibyanaSimCard, LibyanaSimCardDto>(request.Specification,
                    //                                request.PageNumber,
                    //                                request.PageSize,
                    //                                _mapper.ConfigurationProvider,
                    //                                cancellationToken);
                    var data = await _context.LibyanaSimCards.Where(o => !tSimCards.Contains(o.SimCardNo))
                        .OrderBy($"{request.OrderBy} {request.SortDirection}")
               .ProjectToPaginatedDataAsync(request.Specification, request.PageNumber, request.PageSize, Mapper.ToDto, cancellationToken);
                    return data;

                }
            case LibyanaSimCardListView.SimCardsNotExistOnWialon:
                {
                    var wSimCards = await _context.WialonUnits.Select(o => o.SimCardNo).ToListAsync(cancellationToken);

                    //var data = await _context.LibyanaSimCards.Where(o => !wSimCards.Contains(o.SimCardNo))
                    //                           .OrderBy($"{request.OrderBy} {request.SortDirection}")
                    //                             .ProjectToPaginatedDataAsync<LibyanaSimCard, LibyanaSimCardDto>(request.Specification,
                    //                                request.PageNumber,
                    //                                request.PageSize,
                    //                                _mapper.ConfigurationProvider,
                    //                                cancellationToken);
                    var data = await _context.LibyanaSimCards.Where(o => !wSimCards.Contains(o.SimCardNo))
                        .OrderBy($"{request.OrderBy} {request.SortDirection}")
               .ProjectToPaginatedDataAsync(request.Specification, request.PageNumber, request.PageSize, Mapper.ToDto, cancellationToken);
                    return data;

                }
            default:
                {
                    return null;
                }
        }
    }
}