namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to activate or deactivate a unit
/// </summary>
public record ActivateUnitCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the unit to activate/deactivate
    /// </summary>
    public int ItemId { get; init; }
    
    /// <summary>
    /// Whether to activate (true) or deactivate (false) the unit
    /// </summary>
    public bool Active { get; init; } = true;
}

/// <summary>
/// Handler for ActivateUnitCommand
/// </summary>
public class ActivateUnitCommandHandler : IRequestHandler<ActivateUnitCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<ActivateUnitCommandHandler> _logger;

    public ActivateUnitCommandHandler(
        IWialonService wialonService, 
        ILogger<ActivateUnitCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async ValueTask<Result<bool>> Handle(ActivateUnitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Setting unit {ItemId} active status to {Active}", 
                request.ItemId, request.Active);
            
            // Wialon API expects "1" for active, "0" for inactive
            var activeValue = request.Active ? "1" : "0";
            var response = await _wialonService.ActivateUnit(request.ItemId, activeValue, cancellationToken);

            if (response?.IsSuccess == true)
            {
                var status = request.Active ? "activated" : "deactivated";
                _logger.LogInformation("Unit {ItemId} {Status} successfully", request.ItemId, status);
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                $"Failed to update unit status: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating unit status");
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}