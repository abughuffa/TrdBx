using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Commands.RetrieveToken;

public record RetrieveTokenCommand : IRequest<Result<bool>>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class RetrieveTokenCommandHandler : IRequestHandler<RetrieveTokenCommand, Result<bool>>
{
    private readonly IWialonService _wialonService;
    private readonly ILogger<RetrieveTokenCommandHandler> _logger;

    public RetrieveTokenCommandHandler(IWialonService wialonService, ILogger<RetrieveTokenCommandHandler> logger)
    {
        _wialonService = wialonService;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(RetrieveTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Result<bool>.Failure("Username and password are required to retrieve token");
            }

            _logger.LogInformation("Attempting to retrieve token for user {User}", request.Username);

            var token = await _wialonService.RetrieveTokenAsync(request.Username, request.Password, cancellationToken);

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("Token retrieval failed or returned empty token for user {User}", request.Username);
                return Result<bool>.Failure("Failed to retrieve token from CMS login endpoint");
            }

            _logger.LogInformation("Token retrieved successfully for user {User}, updating Wialon service", request.Username);

            var success = await _wialonService.UpdateTokenAsync(token, cancellationToken);
            if (success)
            {
                _logger.LogInformation("Wialon token updated and session re-established");
                return Result<bool>.Success(true);
            }

            _logger.LogWarning("Failed to update Wialon service with new token");
            return Result<bool>.Failure("Retrieved token but failed to update Wialon service");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving token");
            return Result<bool>.Failure($"Error while retrieving token: {ex.Message}");
        }
    }
}