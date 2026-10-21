// using MediatR;
using CleanArchitecture.Blazor.Domain;

namespace CleanArchitecture.Blazor.Application.Features.Wialon.Queries;

/// <summary>
/// Query to get Wialon service health status
/// </summary>
public record GetWialonHealthQuery : IRequest<WialonHealthStatus>;

/// <summary>
/// Handler for GetWialonHealthQuery
/// </summary>
public class GetWialonHealthQueryHandler : IRequestHandler<GetWialonHealthQuery, WialonHealthStatus>
{
    private readonly IWialonService _wialonService;

    public GetWialonHealthQueryHandler(IWialonService wialonService)
    {
        _wialonService = wialonService;
    }

    public async ValueTask<WialonHealthStatus> Handle(GetWialonHealthQuery request, CancellationToken cancellationToken)
    {
        // Delegate to the Wialon service
        return await _wialonService.GetHealthStatusAsync();
    }
}

/// <summary>
/// Query to get session information
/// </summary>
public record GetSessionInfoQuery : IRequest<SessionInfo>;

/// <summary>
/// Session information object
/// </summary>
public class SessionInfo
{
    /// <summary>
    /// Current session ID
    /// </summary>
    public string? SessionId { get; set; }
    
    /// <summary>
    /// Whether the session is valid
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// Whether the session is active
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// Handler for GetSessionInfoQuery
/// </summary>
public class GetSessionInfoQueryHandler : IRequestHandler<GetSessionInfoQuery, SessionInfo>
{
    private readonly IWialonService _wialonService;

    public GetSessionInfoQueryHandler(IWialonService wialonService)
    {
        _wialonService = wialonService;
    }

    public async ValueTask<SessionInfo> Handle(GetSessionInfoQuery request, CancellationToken cancellationToken)
    {
        // Get session information from the Wialon service
        var sessionId = await _wialonService.GetSessionIdAsync();
        var isValid = await _wialonService.IsSessionValidAsync();
        
        return new SessionInfo
        {
            SessionId = sessionId,
            IsValid = isValid,
            IsActive = !string.IsNullOrEmpty(sessionId)
        };
    }
}