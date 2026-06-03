
namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands;

/// <summary>
/// Command to delete an item by its ID
/// </summary>
public record DeleteItemCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// ID of the item to delete
    /// </summary>
    public int ItemId { get; init; }
}

/// <summary>
/// Handler for DeleteItemCommand
/// </summary>
public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<DeleteItemCommandHandler> _logger;

    public DeleteItemCommandHandler(
        IWialonService wialonService, 
        ILogger<DeleteItemCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogWarning("Attempting to delete item {ItemId}", request.ItemId);
            
            // Call the Wialon service to delete the item
            var response = await _wialonService.DeleteItem(request.ItemId, cancellationToken);

            if (response?.IsSuccess == true)
            {
                _logger.LogInformation("Item {ItemId} deleted successfully", request.ItemId);
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                $"Failed to delete item: {response?.ErrorReason ?? "Unknown error"}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting item {ItemId}", request.ItemId);
            return Result<bool>.Failure($"Error: {ex.Message}");
        }
    }
}