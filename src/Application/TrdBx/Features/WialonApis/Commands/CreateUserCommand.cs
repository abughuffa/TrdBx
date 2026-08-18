
namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to create a new user
/// </summary>
public record CreateUserCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the creator/parent resource
    /// </summary>
    public int CreatorId { get; init; }
    
    /// <summary>
    /// Username for the new user
    /// </summary>
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Password for the new user
    /// </summary>
    public string Password { get; init; } = string.Empty;
    
    /// <summary>
    /// Data flags for the user
    /// </summary>
    public string DataFlags { get; init; } = "base";
}

/// <summary>
/// Handler for CreateUserCommand
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IWialonService wialonService, 
        ILogger<CreateUserCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async ValueTask<Result<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating user: {Name} under creator {CreatorId}", 
                request.Name, request.CreatorId);
            
            // Call the Wialon service to create the user
            var response = await _wialonService.CreateUser(
                request.CreatorId,
                request.Name,
                request.Password,
                request.DataFlags,
                cancellationToken);

            if (response?.IsSuccess == true)
            {
                _logger.LogInformation("User {Name} created successfully", request.Name);
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                $"Failed to create user: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}