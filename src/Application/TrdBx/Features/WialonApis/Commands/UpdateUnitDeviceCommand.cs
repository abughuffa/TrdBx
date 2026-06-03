
namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to update a unit's device type and unique identifier
/// </summary>
public record UpdateUnitDeviceCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the unit to update
    /// </summary>
    public int ItemId { get; init; }
    
    /// <summary>
    /// Device type ID (e.g., 1 for GPS tracking device)
    /// </summary>
    public int DeviceTypeId { get; init; }
    
    /// <summary>
    /// Unique identifier for the device (IMEI or serial number)
    /// </summary>
    public string UniqueId { get; init; } = string.Empty;
}

/// <summary>
/// Handler for UpdateUnitDeviceCommand
/// </summary>
public class UpdateUnitDeviceCommandHandler : IRequestHandler<UpdateUnitDeviceCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<UpdateUnitDeviceCommandHandler> _logger;

    public UpdateUnitDeviceCommandHandler(
        IWialonService wialonService, 
        ILogger<UpdateUnitDeviceCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UpdateUnitDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating device for unit {ItemId} to type {DeviceTypeId} with ID {UniqueId}", 
                request.ItemId, request.DeviceTypeId, request.UniqueId);
            
            // Call the Wialon service to update the unit device
            var response = await _wialonService.UpdateUnitDeviceTypeUniqueId(
                request.ItemId,
                request.DeviceTypeId,
                request.UniqueId,
                cancellationToken);

            if (response?.IsSuccess == true)
            {
                _logger.LogInformation("Unit device updated successfully for unit {ItemId}", request.ItemId);
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                $"Failed to update unit device: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating unit device for unit {ItemId}", request.ItemId);
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}