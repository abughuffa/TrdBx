
namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to create a new unit group
/// </summary>
public record CreateUnitGroupCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the creator/parent resource
    /// </summary>
    public int CreatorId { get; init; }
    
    /// <summary>
    /// Name of the unit group
    /// </summary>
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Data flags for the group
    /// </summary>
    public string DataFlags { get; init; } = "base";
}

/// <summary>
/// Handler for CreateUnitGroupCommand
/// </summary>
public class CreateUnitGroupCommandHandler : IRequestHandler<CreateUnitGroupCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<CreateUnitGroupCommandHandler> _logger;

    public CreateUnitGroupCommandHandler(
        IWialonService wialonService, 
        ILogger<CreateUnitGroupCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async ValueTask<Result<bool>> Handle(CreateUnitGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating unit group: {Name} under creator {CreatorId}", 
                request.Name, request.CreatorId);
            
            // Call the Wialon service to create the unit group
            var response = await _wialonService.CreateUnitGroup(
                request.CreatorId,
                request.Name,
                request.DataFlags,
                cancellationToken);

            if (response?.IsSuccess == true)
            {
                _logger.LogInformation("Unit group {Name} created successfully", request.Name);
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                $"Failed to create unit group: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating unit group");
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}