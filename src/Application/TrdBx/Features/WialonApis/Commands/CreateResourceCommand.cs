using CleanArchitecture.Blazor.Domain;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to create a new resource in Wialon
/// </summary>
public record CreateResourceCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the creator/parent resource
    /// </summary>
    public int CreatorId { get; init; }
    
    /// <summary>
    /// Name of the new resource
    /// </summary>
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Data flags for the resource (e.g., "base", "custom")
    /// </summary>
    public string DataFlags { get; init; } = "base";
    
    /// <summary>
    /// Whether to skip creator permission check
    /// </summary>
    public bool SkipCreatorCheck { get; init; } = false;
}

/// <summary>
/// Handler for CreateResourceCommand
/// </summary>
public class CreateResourceCommandHandler : IRequestHandler<CreateResourceCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<CreateResourceCommandHandler> _logger;

    public CreateResourceCommandHandler(
        IWialonService wialonService, 
        ILogger<CreateResourceCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async ValueTask<Result<bool>> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating resource: {Name} under creator {CreatorId}", 
                request.Name, request.CreatorId);
            
            // Call the Wialon service to create the resource
            var response = await _wialonService.CreateResource(
                request.CreatorId,
                request.Name,
                request.DataFlags,
                request.SkipCreatorCheck,
                cancellationToken);

            // Check if the operation was successful
            if (response?.IsSuccess == true)
            {
                _logger.LogInformation("Resource {Name} created successfully", request.Name);
                return Result<bool>.Success(true);
            }

            // Return failure with the error reason from Wialon
            return Result<bool>.Failure(
                $"Failed to create resource: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (WialonApiException ex)
        {
            _logger.LogError(ex, "Wialon API error while creating resource");
            return Result<bool>.Failure($"API Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating resource");
            return Result<bool>.Failure($"Unexpected error: {ex.Message}");
        }
    }
}