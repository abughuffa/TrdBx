
namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to update a unit's phone number (SIM card number)
/// </summary>
public record UpdateUnitPhoneCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the unit to update
    /// </summary>
    public int ItemId { get; init; }
    
    /// <summary>
    /// Phone number to assign to the unit (E.164 format recommended)
    /// </summary>
    public string PhoneNumber { get; init; } = string.Empty;
}

/// <summary>
/// Handler for UpdateUnitPhoneCommand
/// </summary>
public class UpdateUnitPhoneCommandHandler : IRequestHandler<UpdateUnitPhoneCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<UpdateUnitPhoneCommandHandler> _logger;

    public UpdateUnitPhoneCommandHandler(
        IWialonService wialonService, 
        ILogger<UpdateUnitPhoneCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async ValueTask<Result<bool>> Handle(UpdateUnitPhoneCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating phone number for unit {ItemId} to {PhoneNumber}", 
                request.ItemId, request.PhoneNumber);
            
            // Call the Wialon service to update the unit phone number
            var response = await _wialonService.UpdateUnitPhoneNumber(request.ItemId, request.PhoneNumber, cancellationToken);

            if (response?.IsSuccess == true)
            {
                _logger.LogInformation("Unit phone number updated successfully for unit {ItemId}", request.ItemId);
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                $"Failed to update phone number: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating unit phone number for unit {ItemId}", request.ItemId);
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}