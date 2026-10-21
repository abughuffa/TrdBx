
namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to create a new account under a resource
/// </summary>
public record CreateAccountCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the resource item
    /// </summary>
    public int ItemId { get; init; }
    
    /// <summary>
    /// Account plan (e.g., "basic", "premium")
    /// </summary>
    public string Plan { get; init; } = "basic";
}

/// <summary>
/// Handler for CreateAccountCommand
/// </summary>
public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<CreateAccountCommandHandler> _logger;

    public CreateAccountCommandHandler(
        IWialonService wialonService, 
        ILogger<CreateAccountCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async ValueTask<Result<bool>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating account for item {ItemId} with plan {Plan}", 
                request.ItemId, request.Plan);
            
            // Call the Wialon service to create the account
            var response = await _wialonService.CreateAccount(request.ItemId, request.Plan, cancellationToken);

            if (response?.IsSuccess == true)
            {
                _logger.LogInformation("Account created successfully for item {ItemId}", request.ItemId);
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                $"Failed to create account: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating account");
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}