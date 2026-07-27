// DropdownDataService.cs
using CleanArchitecture.Blazor.Application.Features.SimCards.Queries.GetAvaliableSimCards;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Queries.GetAvaliable;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Queries.GetAvaliable;
using CleanArchitecture.Blazor.Application.Features.Customers.Queries.GetAvaliable;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
//using MediatR;
//using MudBlazor;

namespace CleanArchitecture.Blazor.Application.TrdBx.Services
{
    public class DropdownDataService : IDropdownDataService
    {
        private readonly IMediator _mediator;
        //private readonly ISnackbar _snackbar;

        public DropdownDataService(IMediator mediator)//, ISnackbar snackbar)
        {
            _mediator = mediator;
           // _snackbar = snackbar;
        }

        public async Task<IEnumerable<SimCardDto>> GetAvailableSimCardsAsync(int[]? ids = null)
        {
            try
            {
                // The cache is automatically handled by the ICacheableRequest
                return await _mediator.Send(new GetAvaliableSimCardsQuery() { Ids = ids });
            }
            catch //(Exception ex)
            {
                //_snackbar.Add($"Failed to load SIM cards: {ex.Message}", Severity.Warning);
                return Enumerable.Empty<SimCardDto>();
            }
        }

        public async Task<IEnumerable<TrackedAssetDto>> GetAvailableTrackedAssetsAsync()
        {
            try
            {
                return await _mediator.Send(new GetAvaliableTrackedAssetsQuery());
            }
            catch //(Exception ex)
            {
                //_snackbar.Add($"Failed to load tracked assets: {ex.Message}", Severity.Warning);
                return Enumerable.Empty<TrackedAssetDto>();
            }
        }

        public async Task<IEnumerable<CustomerDto>> GetAvailableCustomersAsync(int? customerId = null)
        {
            try
            {
                return await _mediator.Send(new GetAvaliableChildsByParentIdQuery() { Id = customerId });
            }
            catch (Exception ex)
            {
               // _snackbar.Add($"Failed to load customers: {ex.Message}", Severity.Warning);
                return Enumerable.Empty<CustomerDto>();
            }
        }

        public async Task<IEnumerable<TrackingUnitDto>> GetAvailableTrackingUnitsAsync(int? customerId = null)
        {
            try
            {
                return await _mediator.Send(new GetAvaliableTrackingUnitsQuery() { Id = (int)customerId });
            }
            catch (Exception ex)
            {
               // _snackbar.Add($"Failed to load customers: {ex.Message}", Severity.Warning);
                return Enumerable.Empty<TrackingUnitDto>();
            }
        }

        public async Task ClearSimCardsCacheAsync()
        {
            SimCardCacheKey.Refresh();
            await Task.CompletedTask;
        }

        public async Task ClearTrackedAssetsCacheAsync()
        {
            TrackedAssetCacheKey.Refresh();     
            await Task.CompletedTask;
        }

        public async Task ClearCustomersCacheAsync()
        {

                TrackedAssetCacheKey.Refresh();
                await Task.CompletedTask;

        }
        public async Task ClearTrackingUnitsCacheAsync()
        {

                TrackingUnitCacheKey.Refresh();
                await Task.CompletedTask;

        }
    }
}