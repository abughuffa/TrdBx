
// IDropdownDataService.cs
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
    public interface IDropdownDataService
    {
        Task<IEnumerable<SimCardDto>> GetAvailableSimCardsAsync(int[]? ids = null);
        Task<IEnumerable<TrackedAssetDto>> GetAvailableTrackedAssetsAsync();
        Task<IEnumerable<CustomerDto>> GetAvailableCustomersAsync(int? customerId = null);
        Task<IEnumerable<TrackingUnitDto>> GetAvailableTrackingUnitsAsync(int? customerId = null);

        Task ClearSimCardsCacheAsync();
        Task ClearTrackedAssetsCacheAsync();
        Task ClearCustomersCacheAsync();
        Task ClearTrackingUnitsCacheAsync();

        
    }