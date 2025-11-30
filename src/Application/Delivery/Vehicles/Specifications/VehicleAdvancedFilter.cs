
namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Specifications;

#nullable disable warnings

/// <summary>
/// A class for applying advanced filtering options to Vehicle lists.
/// </summary>
public class VehicleAdvancedFilter: PaginationFilter
{
    public UserProfile? CurrentUser { get; set; }
}