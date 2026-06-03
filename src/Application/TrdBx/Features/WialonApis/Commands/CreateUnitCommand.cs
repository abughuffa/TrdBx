
namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to create a new tracking unit
/// </summary>
public record CreateUnitCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the creator/parent resource
    /// </summary>
    public int CreatorId { get; init; }
    
    /// <summary>
    /// Name of the unit
    /// </summary>
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Hardware type ID (1 = Wialon GPS, 2 = Wialon Retranslator, etc.)
    /// </summary>
    public int HwTypeId { get; init; } = 1;
    
    /// <summary>
    /// Data flags for the unit
    /// </summary>
    public string DataFlags { get; init; } = "base";
}

/// <summary>
/// Handler for CreateUnitCommand
/// </summary>
public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<CreateUnitCommandHandler> _logger;

    public CreateUnitCommandHandler(
        IWialonService wialonService, 
        ILogger<CreateUnitCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating unit: {Name} under creator {CreatorId} with hardware type {HwTypeId}", 
                request.Name, request.CreatorId, request.HwTypeId);
            
            // Call the Wialon service to create the unit
            var response = await _wialonService.CreateUnit(
                request.CreatorId,
                request.Name,
                request.HwTypeId,
                request.DataFlags,
                cancellationToken);

            if (response?.IsSuccess == true)
            {
                _logger.LogInformation("Unit {Name} created successfully", request.Name);
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                $"Failed to create unit: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating unit");
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}