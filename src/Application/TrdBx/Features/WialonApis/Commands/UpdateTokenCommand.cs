
namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to update the Wialon API token
/// </summary>
public record UpdateTokenCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// New API token to use
    /// </summary>
    public string NewToken { get; init; } = string.Empty;
}

/// <summary>
/// Handler for UpdateTokenCommand
/// </summary>
public class UpdateTokenCommandHandler : IRequestHandler<UpdateTokenCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<UpdateTokenCommandHandler> _logger;

    public UpdateTokenCommandHandler(
        IWialonService wialonService, 
        ILogger<UpdateTokenCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UpdateTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate token is not empty
            if (string.IsNullOrWhiteSpace(request.NewToken))
            {
                return Result<bool>.Failure("Token cannot be empty");
            }

            _logger.LogInformation("Updating Wialon API token");
            
            // Call the Wialon service to update the token
            var success = await _wialonService.UpdateTokenAsync(request.NewToken, cancellationToken);
            
            if (success)
            {
                _logger.LogInformation("Token updated and session re-established successfully");
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure("Failed to update token");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating token");
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}