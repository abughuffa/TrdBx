using System.Diagnostics;
using System.Text;
using System.Text.Json;
using CleanArchitecture.Blazor.Domain;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

/// <summary>
/// Service implementation for Wialon API integration
/// Manages session lifecycle and executes API calls
/// </summary>
public class WialonService : IWialonService, IDisposable
{
    // ========== FIELDS ==========
    
    private readonly HttpClient _httpClient;
    private readonly WialonSessionConfig _config;
    private readonly ILogger<WialonService> _logger;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    
    // Session state
    private string? _sessionId;
    private Timer? _keepAliveTimer;
    private Timer? _healthCheckTimer;
    private bool _isSessionActive;
    private DateTime _lastActivity;
    private DateTime _sessionStartTime;
    private CancellationTokenSource? _cts;
    private bool _disposed;
    private string _currentToken;
    
    // Metrics and logging
    private readonly List<WialonApiCall> _apiCalls = new();
    private int _totalApiCalls;
    private int _failedApiCalls;
    
    // Wialon specific error codes
    private const int ERROR_SESSION_EXPIRED = 1;      // Session not found or expired
    private const int ERROR_INVALID_SESSION = 6;       // Invalid session
    private const int ERROR_TOKEN_EXPIRED = 7;         // Token expired
    private const int ERROR_ACCESS_DENIED = 3;         // Access denied
    private const int ERROR_INVALID_PARAMS = 4;        // Invalid parameters
    
    // ========== EVENTS ==========
    
    public event EventHandler<WialonSessionEventArgs>? SessionStatusChanged;
    public event EventHandler<WialonSessionEventArgs>? SessionExpired;
    public event EventHandler<WialonSessionEventArgs>? TokenExpiring;

    // ========== CONSTRUCTOR ==========
    
    public WialonService(
        HttpClient httpClient,
        IOptions<WialonSessionConfig> config,
        ILogger<WialonService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currentToken = _config.Token;
        
        // Configure HTTP client
        _httpClient.BaseAddress = new Uri(_config.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "BlazorServer-WialonClient/1.0");
    }

    // ========== SESSION MANAGEMENT ==========

    
    /// <summary>
    /// Logs into Wialon using the configured token
    /// </summary>
    public async Task<WialonLoginResult> LoginAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            // If session is active, log out first
            if (_isSessionActive)
            {
                _logger.LogInformation("Session already active, logging out first");
                await LogoutInternalAsync(cancellationToken);
            }

            return await PerformLoginAsync(cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Performs the actual login operation to Wialon
    /// </summary>
    private async Task<WialonLoginResult> PerformLoginAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting Wialon login with token");
        
        var loginResult = new WialonLoginResult();

        try
        {
            var attemptCount = 0;
            var success = false;

            // Retry logic with exponential backoff
            while (!success && attemptCount < _config.MaxRetryAttempts)
            {
                attemptCount++;
                try
                {
                    // Build login URL with token parameter (URL encoded for safety)
                    var tokenParam = Uri.EscapeDataString(_currentToken);
                    var url = $"/wialon/ajax.html?svc=token/login&params={{\"token\":\"{tokenParam}\"}}";
                    
                    _logger.LogDebug("Login attempt {Attempt} to {BaseUrl}", attemptCount, _config.BaseUrl);
                    
                    // Execute login request
                    var response = await _httpClient.GetAsync(url, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogDebug("Login response received");
                    
                    // Deserialize response
                    var loginResponse = JsonSerializer.Deserialize<WialonLoginResponse>(content);

                    if (loginResponse?.IsSuccess == true && !string.IsNullOrEmpty(loginResponse.SessionId))
                    {
                        // Login successful - store session info
                        _sessionId = loginResponse.SessionId;
                        _isSessionActive = true;
                        _lastActivity = DateTime.UtcNow;
                        _sessionStartTime = DateTime.UtcNow;

                        loginResult.Success = true;
                        loginResult.SessionId = loginResponse.SessionId;
                        loginResult.Host = loginResponse.Host;
                        loginResult.User = loginResponse.User;

                        // Start background timers
                        StartKeepAlive();
                        StartHealthCheck();
                        
                        // Raise event
                        OnSessionStatusChanged(new WialonSessionEventArgs
                        {
                            SessionId = _sessionId,
                            IsActive = true,
                            Message = "Login successful",
                            Timestamp = DateTime.UtcNow
                        });

                        _logger.LogInformation("Wialon login successful. Session ID: {SessionId}, User: {UserName}", 
                            _sessionId, loginResponse.User?.Name);
                        success = true;
                    }
                    else
                    {
                        // Login failed
                        var errorMsg = $"Login failed - Error Code: {loginResponse?.ErrorCode}, Reason: {loginResponse?.ErrorReason}";
                        _logger.LogWarning(errorMsg);
                        
                        // Check if token is invalid/expired - don't retry if token is the issue
                        if (loginResponse?.ErrorCode == ERROR_TOKEN_EXPIRED || 
                            loginResponse?.ErrorCode == ERROR_ACCESS_DENIED)
                        {
                            loginResult.ErrorMessage = $"Token is invalid or expired: {errorMsg}";
                            OnTokenExpiring(new WialonSessionEventArgs
                            {
                                SessionId = null,
                                IsActive = false,
                                Message = "Token has expired or is invalid. Please update your API token.",
                                Timestamp = DateTime.UtcNow
                            });
                            break; // Exit retry loop
                        }
                        
                        // Retry if attempts remain
                        if (attemptCount >= _config.MaxRetryAttempts)
                        {
                            loginResult.ErrorMessage = errorMsg;
                        }
                        else
                        {
                            var delay = _config.RetryDelayMilliseconds * attemptCount;
                            _logger.LogDebug("Retrying login in {Delay}ms", delay);
                            await Task.Delay(delay, cancellationToken);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "HTTP error during login attempt {Attempt}", attemptCount);
                    
                    if (attemptCount >= _config.MaxRetryAttempts)
                    {
                        loginResult.ErrorMessage = $"Network error after {attemptCount} attempts: {ex.Message}";
                    }
                    else
                    {
                        var delay = _config.RetryDelayMilliseconds * attemptCount;
                        await Task.Delay(delay, cancellationToken);
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "JSON deserialization error during login");
                    loginResult.ErrorMessage = $"Invalid response format: {ex.Message}";
                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
            loginResult.ErrorMessage = "Login operation was cancelled";
            _logger.LogWarning("Login operation cancelled");
        }
        catch (Exception ex)
        {
            loginResult.ErrorMessage = $"Unexpected error during login: {ex.Message}";
            _logger.LogError(ex, "Unexpected error during login");
        }

        return loginResult;
    }

    /// <summary>
    /// Logs out from the current Wialon session
    /// </summary>
    public async Task<bool> LogoutAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            return await LogoutInternalAsync(cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Internal logout implementation
    /// </summary>
    private async Task<bool> LogoutInternalAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Logging out from Wialon session");

        try
        {
            // Stop background tasks
            StopKeepAlive();
            StopHealthCheck();

            // Call Wialon logout API if session exists
            if (!string.IsNullOrEmpty(_sessionId))
            {
                var url = $"/wialon/ajax.html?svc=core/logout&sid={_sessionId}";
                var response = await _httpClient.GetAsync(url, cancellationToken);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully logged out session: {SessionId}", _sessionId);
                }
                else
                {
                    _logger.LogWarning("Logout request failed with status: {StatusCode}", response.StatusCode);
                }
            }

            // Clear session state
            _sessionId = null;
            _isSessionActive = false;

            // Raise event
            OnSessionStatusChanged(new WialonSessionEventArgs
            {
                SessionId = null,
                IsActive = false,
                Message = "Logged out successfully",
                Timestamp = DateTime.UtcNow
            });

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return false;
        }
    }

    /// <summary>
    /// Attempts to retrieve a new API token by submitting credentials to the CMS login endpoint.
    /// The CMS login endpoint issues an access_token as a query parameter on redirect when login succeeds.
    /// This method performs a POST with form data (password) and inspects the Location header for access_token.
    /// </summary>
    public async Task<string?> RetrieveTokenAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        try
        {
            var loginPath = $"/login.html?client_id=TrdBx&access_type=-1&activation_time=0&duration=2592000&flags=1&user={Uri.EscapeDataString(username)}";

            using var handler = new HttpClientHandler { AllowAutoRedirect = false };
            using var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(_config.BaseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };

            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("password", password)
            });

            var response = await client.PostAsync(loginPath, form, cancellationToken);

            if (response.Headers.Location != null)
            {
                var location = response.Headers.Location;
                var query = location.Query;
                var parsed = QueryHelpers.ParseQuery(query);

                if (parsed.TryGetValue("access_token", out var tokenValues))
                {
                    var token = tokenValues.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(token))
                        return token;
                }

                var abs = location.IsAbsoluteUri
                    ? location.AbsoluteUri
                    : new Uri(client.BaseAddress!, location.ToString()).AbsoluteUri;

                var fragIndex = abs.IndexOf('#');
                if (fragIndex >= 0)
                {
                    var frag = abs[(fragIndex + 1)..];
                    var fragParsed = QueryHelpers.ParseQuery("?" + frag);
                    if (fragParsed.TryGetValue("access_token", out var fragToken))
                        return fragToken.FirstOrDefault();
                }
            }

            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!string.IsNullOrEmpty(body))
            {
                var m = Regex.Match(body, "access_token=([^&\"']+)", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    return Uri.UnescapeDataString(m.Groups[1].Value);
                }

                m = Regex.Match(body, "\"access_token\"\\s*:\\s*\"(?<tok>[^\"]+)\"",
                    RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    return m.Groups["tok"].Value;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving token from CMS login endpoint");
            return null;
        }
    }

    /// <summary>
    /// Gets the current session ID if session is active
    /// </summary>
    public async Task<string?> GetSessionIdAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            return (_isSessionActive && !string.IsNullOrEmpty(_sessionId)) ? _sessionId : null;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Checks if the current session is still valid
    /// </summary>
    public async Task<bool> IsSessionValidAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            // Basic session existence check
            if (!_isSessionActive || string.IsNullOrEmpty(_sessionId))
            {
                _logger.LogDebug("Session invalid: Active={IsActive}, HasSessionId={HasSessionId}", 
                    _isSessionActive, !string.IsNullOrEmpty(_sessionId));
                return false;
            }

            // Check inactivity timeout
            var inactivityTime = DateTime.UtcNow - _lastActivity;
            if (inactivityTime.TotalMinutes >= _config.SessionTimeoutMinutes)
            {
                _logger.LogWarning("Session inactive for {InactivityMinutes:F1} minutes (limit: {TimeoutMinutes})", 
                    inactivityTime.TotalMinutes, _config.SessionTimeoutMinutes);
                return false;
            }
            
            // Check session age - Wialon sessions typically last up to 24 hours
            var sessionAge = DateTime.UtcNow - _sessionStartTime;
            if (sessionAge.TotalHours >= 23)
            {
                _logger.LogWarning("Session age is {SessionAgeHours:F1} hours, approaching limit", sessionAge.TotalHours);
                // Still return true but log warning for monitoring
            }
            
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Ensures a valid session exists, reconnecting if necessary
    /// </summary>
    public async Task<bool> EnsureSessionAsync(CancellationToken cancellationToken = default)
    {
        if (!await IsSessionValidAsync())
        {
            _logger.LogWarning("Session invalid or expired, attempting to reconnect");
            var loginResult = await LoginAsync(cancellationToken);
            return loginResult.Success;
        }
        
        _logger.LogDebug("Session is valid");
        return true;
    }

    /// <summary>
    /// Updates the API token and re-establishes session
    /// </summary>
    public async Task<bool> UpdateTokenAsync(string newToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(newToken))
        {
            throw new ArgumentException("Token cannot be empty", nameof(newToken));
        }
        
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            _logger.LogInformation("Updating Wialon API token");
            _currentToken = newToken;
            
            // Re-login with new token if session was active
            if (_isSessionActive)
            {
                await LogoutInternalAsync(cancellationToken);
                var loginResult = await PerformLoginAsync(cancellationToken);
                return loginResult.Success;
            }
            
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Validates if the current token is still valid
    /// </summary>
    public async Task<bool> ValidateCurrentTokenAsync(CancellationToken cancellationToken = default)
    {
        // Test token by attempting a login (but don't keep the session)
        try
        {
            var tokenParam = Uri.EscapeDataString(_currentToken);
            var url = $"/wialon/ajax.html?svc=token/login&params={{\"token\":\"{tokenParam}\"}}";
            var response = await _httpClient.GetAsync(url, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
                return false;
                
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var loginResponse = JsonSerializer.Deserialize<WialonLoginResponse>(content);
            
            // If we got a session, immediately log it out
            if (loginResponse?.IsSuccess == true && !string.IsNullOrEmpty(loginResponse.SessionId))
            {
                var logoutUrl = $"/wialon/ajax.html?svc=core/logout&sid={loginResponse.SessionId}";
                await _httpClient.GetAsync(logoutUrl, cancellationToken);
                return true;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token validation failed");
            return false;
        }
    }

    /// <summary>
    /// Gets detailed health status
    /// </summary>
    public async Task<WialonHealthStatus> GetHealthStatusAsync()
    {
        return new WialonHealthStatus
        {
            IsHealthy = _isSessionActive && await IsSessionValidAsync(),
            IsSessionActive = _isSessionActive,
            SessionId = _sessionId,
            LastActivity = _lastActivity,
            LastHealthCheck = DateTime.UtcNow,
            RecentApiCalls = _apiCalls.TakeLast(50).ToList(),
            TotalApiCalls = _totalApiCalls,
            FailedApiCalls = _failedApiCalls,
            SuccessRate = _totalApiCalls > 0 
                ? (double)(_totalApiCalls - _failedApiCalls) / _totalApiCalls * 100 
                : 100
        };
    }

    // ========== API REQUEST EXECUTION ==========
    
    /// <summary>
    /// Executes a Wialon API request with retry logic
    /// </summary>
    private async Task<WialonResponse?> ExecuteRequestAsync(
        string serviceName,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var apiCall = new WialonApiCall { ServiceName = serviceName, Timestamp = DateTime.UtcNow };
        
        var attemptCount = 0;
        
        while (attemptCount < _config.MaxRetryAttempts)
        {
            attemptCount++;
            try
            {
                // Ensure we have a valid session before making the call
                if (!await EnsureSessionAsync(cancellationToken))
                {
                    throw new InvalidOperationException("Cannot establish valid session for API call");
                }
                
                var sessionId = await GetSessionIdAsync();
                if (string.IsNullOrEmpty(sessionId))
                {
                    throw new InvalidOperationException("No valid session available for API call");
                }

                // Build request URL
                var paramsJson = parameters != null ? JsonSerializer.Serialize(parameters) : "{}";
                var url = $"/wialon/ajax.html?svc={serviceName}&sid={sessionId}&params={Uri.EscapeDataString(paramsJson)}";

                _logger.LogDebug("Executing Wialon API call: {Service} (Attempt {Attempt})", serviceName, attemptCount);

                // Execute request
                var response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonSerializer.Deserialize<WialonResponse>(content);

                if (result == null)
                {
                    throw new InvalidOperationException($"Failed to deserialize response for service {serviceName}");
                }

                // Check for session expired errors and handle them
                if (result.ErrorCode == ERROR_SESSION_EXPIRED || result.ErrorCode == ERROR_INVALID_SESSION)
                {
                    _logger.LogWarning("Session error detected (Code: {ErrorCode}), invalidating session...", result.ErrorCode);
                    _isSessionActive = false;
                    _sessionId = null;
                    
                    if (attemptCount < _config.MaxRetryAttempts)
                    {
                        _logger.LogInformation("Retrying after session invalidation (Attempt {Attempt}/{Max})", 
                            attemptCount + 1, _config.MaxRetryAttempts);
                        continue; // Retry with new session
                    }
                }
                
                // Check for token expired
                if (result.ErrorCode == ERROR_TOKEN_EXPIRED)
                {
                    _logger.LogError("Token expired, need to update token");
                    OnTokenExpiring(new WialonSessionEventArgs
                    {
                        SessionId = sessionId,
                        IsActive = false,
                        Message = "API token has expired. Please update your token.",
                        Timestamp = DateTime.UtcNow
                    });
                    throw new WialonApiException(result.ErrorCode, "Token expired. Please update your API token.");
                }

                // Update last activity timestamp
                _lastActivity = DateTime.UtcNow;
                result.EnsureSuccess();
                
                // Record successful API call metrics
                stopwatch.Stop();
                apiCall.IsSuccess = true;
                apiCall.DurationMs = stopwatch.ElapsedMilliseconds;
                _totalApiCalls++;
                RecordApiCall(apiCall);
                
                _logger.LogDebug("API call {Service} completed in {Duration}ms", serviceName, stopwatch.ElapsedMilliseconds);
                
                return result;
            }
            catch (HttpRequestException ex) when (attemptCount < _config.MaxRetryAttempts)
            {
                stopwatch.Stop();
                _logger.LogWarning(ex, "HTTP request failed, attempt {Attempt} of {Max}", 
                    attemptCount, _config.MaxRetryAttempts);
                    
                apiCall.IsSuccess = false;
                apiCall.ErrorCode = -1;
                apiCall.DurationMs = stopwatch.ElapsedMilliseconds;
                RecordApiCall(apiCall);
                
                // Exponential backoff
                var delay = _config.RetryDelayMilliseconds * attemptCount;
                await Task.Delay(delay, cancellationToken);
                stopwatch.Restart();
            }
            catch (WialonApiException) when (attemptCount < _config.MaxRetryAttempts)
            {
                _logger.LogWarning("Wialon API error, attempt {Attempt} of {Max}", 
                    attemptCount, _config.MaxRetryAttempts);
                    
                apiCall.IsSuccess = false;
                apiCall.DurationMs = stopwatch.ElapsedMilliseconds;
                RecordApiCall(apiCall);
                
                var delay = _config.RetryDelayMilliseconds * attemptCount;
                await Task.Delay(delay, cancellationToken);
                stopwatch.Restart();
            }
        }
        
        // Record failed API call
        apiCall.IsSuccess = false;
        _failedApiCalls++;
        RecordApiCall(apiCall);
        
        throw new WialonApiException(-1, $"Request failed after {_config.MaxRetryAttempts} attempts");
    }

    /// <summary>
    /// Executes a batch request to Wialon (multiple API calls in one HTTP request)
    /// </summary>
    private async Task<List<WialonResponse>?> ExecuteBatchRequestAsync(
        object[] requests,
        CancellationToken cancellationToken = default)
    {
        await EnsureSessionAsync(cancellationToken);
        
        var sessionId = await GetSessionIdAsync();
        if (string.IsNullOrEmpty(sessionId))
        {
            throw new InvalidOperationException("No valid session available for batch request");
        }

        var jsonContent = JsonSerializer.Serialize(requests);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        var url = $"/wialon/ajax.html?svc=core/batch&sid={sessionId}";
        var response = await _httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        
        // Batch returns an array of responses
        var results = JsonSerializer.Deserialize<List<WialonResponse>>(responseContent);

        if (results == null)
        {
            throw new InvalidOperationException("Failed to deserialize batch response");
        }

        _lastActivity = DateTime.UtcNow;
        return results;
    }

    /// <summary>
    /// Records an API call for health monitoring
    /// </summary>
    private void RecordApiCall(WialonApiCall apiCall)
    {
        lock (_apiCalls)
        {
            _apiCalls.Add(apiCall);
            // Keep only last 100 calls
            while (_apiCalls.Count > 100)
            {
                _apiCalls.RemoveAt(0);
            }
        }
    }

    // ========== KEEP ALIVE & HEALTH CHECK ==========
    
    /// <summary>
    /// Sends a keep-alive request to prevent session timeout
    /// </summary>
    private async Task SendKeepAliveAsync()
    {
        try
        {
            if (!_isSessionActive || string.IsNullOrEmpty(_sessionId))
                return;

            // Use a lightweight API call to keep session alive
            var url = $"/wialon/ajax.html?svc=core/update_data_flags&sid={_sessionId}&params={{\"spec\":{{\"type\":\"user\"}},\"flags\":1}}";
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                _lastActivity = DateTime.UtcNow;
                _logger.LogTrace("Keep-alive sent successfully");
            }
            else
            {
                _logger.LogWarning("Keep-alive failed with status: {StatusCode}", response.StatusCode);
                await HandleSessionExpiry();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending keep-alive");
            await HandleSessionExpiry();
        }
    }

    /// <summary>
    /// Performs periodic health check on the session
    /// </summary>
    private async Task PerformHealthCheckAsync()
    {
        try
        {
            var isValid = await IsSessionValidAsync();
            if (!isValid && _isSessionActive)
            {
                _logger.LogWarning("Health check failed - session invalid");
                await HandleSessionExpiry();
            }
            else if (isValid && _isSessionActive)
            {
                // Check session age and log warning if approaching limit
                var sessionAge = DateTime.UtcNow - _sessionStartTime;
                if (sessionAge.TotalHours >= 23)
                {
                    _logger.LogWarning("Session is old ({AgeHours:F1} hours), consider reconnecting", sessionAge.TotalHours);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check error");
        }
    }

    /// <summary>
    /// Handles session expiry by cleaning up and attempting reconnect if configured
    /// </summary>
    private async Task HandleSessionExpiry()
    {
        var oldSessionId = _sessionId;
        _isSessionActive = false;
        _sessionId = null;
        StopKeepAlive();

        OnSessionExpired(new WialonSessionEventArgs
        {
            SessionId = oldSessionId,
            IsActive = false,
            Message = "Session expired due to inactivity or connection error",
            Timestamp = DateTime.UtcNow
        });

        if (_config.AutoReconnect)
        {
            _logger.LogInformation("Auto-reconnect enabled, attempting to re-login");
            try
            {
                // Small delay before reconnecting to avoid rapid reconnection attempts
                await Task.Delay(1000);
                await LoginAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Auto-reconnect failed");
            }
        }
    }

    /// <summary>
    /// Starts the keep-alive timer
    /// </summary>
    private void StartKeepAlive()
    {
        StopKeepAlive();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        
        _keepAliveTimer = new Timer(
            async _ => 
            {
                if (!token.IsCancellationRequested)
                {
                    await SendKeepAliveAsync();
                }
            },
            null,
            TimeSpan.FromMinutes(_config.KeepAliveIntervalMinutes),
            TimeSpan.FromMinutes(_config.KeepAliveIntervalMinutes));
        
        _logger.LogInformation("Keep-alive timer started with {Interval} minute interval", 
            _config.KeepAliveIntervalMinutes);
    }

    /// <summary>
    /// Starts the health check timer
    /// </summary>
    private void StartHealthCheck()
    {
        StopHealthCheck();
        
        _healthCheckTimer = new Timer(
            async _ => await PerformHealthCheckAsync(),
            null,
            TimeSpan.FromSeconds(_config.HealthCheckIntervalSeconds),
            TimeSpan.FromSeconds(_config.HealthCheckIntervalSeconds));
        
        _logger.LogDebug("Health check timer started with {Interval} second interval", 
            _config.HealthCheckIntervalSeconds);
    }

    /// <summary>
    /// Stops the keep-alive timer
    /// </summary>
    private void StopKeepAlive()
    {
        if (_keepAliveTimer != null)
        {
            _keepAliveTimer.Dispose();
            _keepAliveTimer = null;
        }
        
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    /// <summary>
    /// Stops the health check timer
    /// </summary>
    private void StopHealthCheck()
    {
        if (_healthCheckTimer != null)
        {
            _healthCheckTimer.Dispose();
            _healthCheckTimer = null;
        }
    }

    // ========== EVENT HANDLERS ==========
    
    protected virtual void OnSessionStatusChanged(WialonSessionEventArgs e)
    {
        SessionStatusChanged?.Invoke(this, e);
    }

    protected virtual void OnSessionExpired(WialonSessionEventArgs e)
    {
        SessionExpired?.Invoke(this, e);
    }

    protected virtual void OnTokenExpiring(WialonSessionEventArgs e)
    {
        TokenExpiring?.Invoke(this, e);
    }

    // ========== WIALON API METHODS ==========
    
    /// <summary>
    /// Creates a new resource in Wialon
    /// </summary>
    public async Task<WialonResponse?> CreateResource(int creatorId, string name, string dataFlags, bool skipCreatorCheck, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            creatorId = creatorId,
            name = name,
            dataFlags = dataFlags,
            skipCreatorCheck = skipCreatorCheck ? 1 : 0
        };

        return await ExecuteRequestAsync("core/create_resource", param, cancellationToken);
    }

    /// <summary>
    /// Creates a new account under a resource
    /// </summary>
    public async Task<WialonResponse?> CreateAccount(int itemId, string plan, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            itemId = itemId,
            plan = plan
        };

        return await ExecuteRequestAsync("account/create_account", param, cancellationToken);
    }

    /// <summary>
    /// Creates a new unit group
    /// </summary>
    public async Task<WialonResponse?> CreateUnitGroup(int creatorId, string name, string dataFlags, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            creatorId = creatorId,
            name = name,
            dataFlags = dataFlags
        };

        return await ExecuteRequestAsync("core/create_unit_group", param, cancellationToken);
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    public async Task<WialonResponse?> CreateUser(int creatorId, string name, string password, string dataFlags, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            creatorId = creatorId,
            name = name,
            password = password,
            dataFlags = dataFlags
        };

        return await ExecuteRequestAsync("core/create_user", param, cancellationToken);
    }

    /// <summary>
    /// Creates a new tracking unit
    /// </summary>
    public async Task<WialonResponse?> CreateUnit(int creatorId, string name, int hwTypeId, string dataFlags, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            creatorId = creatorId,
            name = name,
            hwTypeId = hwTypeId,
            dataFlags = dataFlags
        };

        return await ExecuteRequestAsync("core/create_unit", param, cancellationToken);
    }

    /// <summary>
    /// Activates or deactivates a unit
    /// </summary>
    public async Task<WialonResponse?> ActivateUnit(int itemId, string active, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            itemId = itemId,
            active = active
        };

        return await ExecuteRequestAsync("unit/set_active", param, cancellationToken);
    }

    /// <summary>
    /// Deletes an item by ID
    /// </summary>
    public async Task<WialonResponse?> DeleteItem(int itemId, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            itemId = itemId
        };

        return await ExecuteRequestAsync("item/delete_item", param, cancellationToken);
    }

    /// <summary>
    /// Updates a unit's device type and unique identifier
    /// </summary>
    public async Task<WialonResponse?> UpdateUnitDeviceTypeUniqueId(int itemId, int deviceTypeId, string uniqueId, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            itemId = itemId,
            deviceTypeId = deviceTypeId,
            uniqueId = uniqueId
        };

        return await ExecuteRequestAsync("unit/update_device_type", param, cancellationToken);
    }

    /// <summary>
    /// Updates hardware parameters for a unit
    /// </summary>
    public async Task<WialonResponse?> UpdateHardwareParameters(string hwId, string action, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            hwId = hwId,
            action = action
        };

        return await ExecuteRequestAsync("unit/update_hw_params", param, cancellationToken);
    }

    /// <summary>
    /// Updates a unit's phone number
    /// </summary>
    public async Task<WialonResponse?> UpdateUnitPhoneNumber(int itemId, string phoneNumber, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            itemId = itemId,
            phoneNumber = phoneNumber
        };

        return await ExecuteRequestAsync("unit/update_phone", param, cancellationToken);
    }

    /// <summary>
    /// Updates a unit's access password
    /// </summary>
    public async Task<WialonResponse?> UpdateUnitAccessPassword(int itemId, string accessPassword, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            itemId = itemId,
            accessPassword = accessPassword
        };

        return await ExecuteRequestAsync("unit/update_access_password", param, cancellationToken);
    }

    /// <summary>
    /// Adds or removes units from a unit group
    /// </summary>
    public async Task<WialonResponse?> AddRemoveUnitToUnitGroup(int itemId, int[] units, CancellationToken cancellationToken = default)
    {
        var param = new
        {
            itemId = itemId,
            units = units
        };

        return await ExecuteRequestAsync("unit_group/update_units", param, cancellationToken);
    }

    // ========== DISPOSAL ==========
    
    public void Dispose()
    {
        if (_disposed) return;
        
        StopKeepAlive();
        StopHealthCheck();
        _semaphore?.Dispose();
        
        // Synchronous logout attempt during disposal
        if (_isSessionActive && !string.IsNullOrEmpty(_sessionId))
        {
            try
            {
                var url = $"/wialon/ajax.html?svc=core/logout&sid={_sessionId}";
                _httpClient.GetAsync(url).Wait(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to logout during dispose");
            }
        }
        
        _disposed = true;
    }
}