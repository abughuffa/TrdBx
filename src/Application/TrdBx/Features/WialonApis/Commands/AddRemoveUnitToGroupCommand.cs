
namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to add or remove units from a unit group
/// </summary>
public record AddRemoveUnitToGroupCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the group to modify
    /// </summary>
    public int GroupId { get; init; }
    
    /// <summary>
    /// Array of unit IDs to add or remove
    /// </summary>
    public int[] UnitIds { get; init; } = Array.Empty<int>();
    
    /// <summary>
    /// Whether to add (true) or remove (false) the units
    /// </summary>
    public bool Add { get; init; } = true;
}

/// <summary>
/// Handler for AddRemoveUnitToGroupCommand
/// </summary>
public class AddRemoveUnitToGroupCommandHandler : IRequestHandler<AddRemoveUnitToGroupCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<AddRemoveUnitToGroupCommandHandler> _logger;

    public AddRemoveUnitToGroupCommandHandler(
        IWialonService wialonService, 
        ILogger<AddRemoveUnitToGroupCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(AddRemoveUnitToGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var operation = request.Add ? "Adding" : "Removing";
            _logger.LogInformation("{Operation} {Count} units to/from group {GroupId}", 
                operation, request.UnitIds.Length, request.GroupId);
            
            // Call the Wialon service to update the unit group
            var response = await _wialonService.AddRemoveUnitToUnitGroup(request.GroupId, request.UnitIds, cancellationToken);

            if (response?.IsSuccess == true)
            {
                var message = request.Add 
                    ? $"Units added to group successfully" 
                    : $"Units removed from group successfully";
                    
                _logger.LogInformation("{Message} for group {GroupId}", message, request.GroupId);
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                $"Failed to update group: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating unit group {GroupId}", request.GroupId);
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}