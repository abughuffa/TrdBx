using CleanArchitecture.Blazor.Domain;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;


/// <summary>
/// Main interface for Wialon API service operations
/// </summary>
public interface IWialonService
{
    // ========== EVENTS ==========
    
    /// <summary>
    /// Event triggered when session status changes
    /// </summary>
    event EventHandler<WialonSessionEventArgs>? SessionStatusChanged;
    
    /// <summary>
    /// Event triggered when session expires
    /// </summary>
    event EventHandler<WialonSessionEventArgs>? SessionExpired;
    
    /// <summary>
    /// Event triggered when token is about to expire
    /// </summary>
    event EventHandler<WialonSessionEventArgs>? TokenExpiring;
    
    // ========== SESSION MANAGEMENT ==========
    
    /// <summary>
    /// Logs into Wialon using the configured token
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Login result containing session info</returns>
    Task<WialonLoginResult> LoginAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs out from current Wialon session
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if logout was successful</returns>
    Task<bool> LogoutAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to retrieve a new token by submitting credentials to the remote CMS login endpoint.
    /// Returns the access_token string when successful, or null when it could not be retrieved.
    /// </summary>
    Task<string?> RetrieveTokenAsync(string username, string password, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the current session ID (if session is active)
    /// </summary>
    /// <returns>Session ID or null if no active session</returns>
    Task<string?> GetSessionIdAsync();
    
    /// <summary>
    /// Checks if the current session is valid
    /// </summary>
    /// <returns>True if session exists and hasn't timed out</returns>
    Task<bool> IsSessionValidAsync();
    
    /// <summary>
    /// Ensures a valid session exists, reconnecting if necessary
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if a valid session exists after operation</returns>
    Task<bool> EnsureSessionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets detailed health status of the Wialon service
    /// </summary>
    /// <returns>Health status information</returns>
    Task<WialonHealthStatus> GetHealthStatusAsync();
    
    /// <summary>
    /// Updates the API token and re-establishes session
    /// </summary>
    /// <param name="newToken">New API token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if token was updated and new session established</returns>
    Task<bool> UpdateTokenAsync(string newToken, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Validates if the current token is still valid with Wialon
    /// </summary>
    /// <returns>True if token is valid</returns>
    Task<bool> ValidateCurrentTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to retrieve a new token by submitting credentials to the remote CMS login endpoint.
    /// Returns the access_token string when successful, or null when it could not be retrieved.
    /// </summary>
    Task<string?> RetrieveTokenAsync(string username, string password, CancellationToken cancellationToken = default);
    
    // ========== RESOURCE MANAGEMENT ==========
    
    /// <summary>
    /// Creates a new resource (company/account) in Wialon
    /// </summary>
    Task<WialonResponse?> CreateResource(int creatorId, string name, string dataFlags, bool skipCreatorCheck, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new account under a resource
    /// </summary>
    Task<WialonResponse?> CreateAccount(int itemId, string plan, CancellationToken cancellationToken = default);
    
    // ========== UNIT MANAGEMENT ==========
    
    /// <summary>
    /// Creates a new unit group for organizing units
    /// </summary>
    Task<WialonResponse?> CreateUnitGroup(int creatorId, string name, string dataFlags, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new user in the system
    /// </summary>
    Task<WialonResponse?> CreateUser(int creatorId, string name, string password, string dataFlags, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new tracking unit
    /// </summary>
    Task<WialonResponse?> CreateUnit(int creatorId, string name, int hwTypeId, string dataFlags, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Activates or deactivates a unit
    /// </summary>
    Task<WialonResponse?> ActivateUnit(int itemId, string active, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes an item by its ID
    /// </summary>
    Task<WialonResponse?> DeleteItem(int itemId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates a unit's device type and unique identifier
    /// </summary>
    Task<WialonResponse?> UpdateUnitDeviceTypeUniqueId(int itemId, int deviceTypeId, string uniqueId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates hardware parameters for a unit
    /// </summary>
    Task<WialonResponse?> UpdateHardwareParameters(string hwId, string action, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates a unit's phone number (SIM card number)
    /// </summary>
    Task<WialonResponse?> UpdateUnitPhoneNumber(int itemId, string phoneNumber, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates a unit's access password
    /// </summary>
    Task<WialonResponse?> UpdateUnitAccessPassword(int itemId, string accessPassword, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Adds or removes units from a unit group
    /// </summary>
    Task<WialonResponse?> AddRemoveUnitToUnitGroup(int itemId, int[] units, CancellationToken cancellationToken = default);
}