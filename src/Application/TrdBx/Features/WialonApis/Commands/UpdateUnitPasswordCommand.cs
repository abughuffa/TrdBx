
namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to update a unit's access password
/// </summary>
public record UpdateUnitPasswordCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the unit to update
    /// </summary>
    public int ItemId { get; init; }
    
    /// <summary>
    /// New access password for the unit
    /// </summary>
    public string AccessPassword { get; init; } = string.Empty;
}

/// <summary>
/// Handler for UpdateUnitPasswordCommand
/// </summary>
public class UpdateUnitPasswordCommandHandler : IRequestHandler<UpdateUnitPasswordCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<UpdateUnitPasswordCommandHandler> _logger;

    public UpdateUnitPasswordCommandHandler(
        IWialonService wialonService, 
        ILogger<UpdateUnitPasswordCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UpdateUnitPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating access password for unit {ItemId}", request.ItemId);
            
            // Call the Wialon service to update the unit password
            var response = await _wialonService.UpdateUnitAccessPassword(request.ItemId, request.AccessPassword, cancellationToken);

            if (response?.IsSuccess == true)
            {
                _logger.LogInformation("Unit access password updated successfully for unit {ItemId}", request.ItemId);
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                $"Failed to update password: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating unit password for unit {ItemId}", request.ItemId);
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}