
// IDropdownDataService.cs
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
    public interface IDropdownDataService
    {
        Task<IEnumerable<SimCardDto>> GetAvailableSimCardsAsync(int[]? ids = null);
        Task<IEnumerable<TrackedAssetDto>> GetAvailableTrackedAssetsAsync(int? id = null);
        Task<IEnumerable<CustomerDto>> GetAvailableCustomersAsync(int? customerId = null);
        Task<IEnumerable<TrackingUnitDto>> GetAvailableTrackingUnitsAsync(int? customerId = null);
        Task<IEnumerable<SmsConfigTemplateDto>> GetSmsConfigTemplatesAsync(int modelId);

        Task ClearSimCardsCacheAsync();
        Task ClearTrackedAssetsCacheAsync();
        Task ClearCustomersCacheAsync();
        Task ClearTrackingUnitsCacheAsync();
        Task ClearSmsConfigTemplatesCacheAsync();

        
    }