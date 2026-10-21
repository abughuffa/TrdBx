using Microsoft.Extensions.Hosting;
using CleanArchitecture.Blazor.Domain;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

/// <summary>
/// Background service that manages Wialon session lifecycle
/// Ensures session is maintained and automatically reconnects when needed
/// </summary>
public class WialonSessionBackgroundService : BackgroundService, IDisposable
{
    private readonly IWialonService _sessionService;
    private readonly ILogger<WialonSessionBackgroundServicex> _logger;
    private readonly SemaphoreSlim _startupLock = new(1, 1);
    private bool _isStarted;
    private bool _disposed;
    
    // Event handlers for cleanup
    private EventHandler<WialonSessionEventArgs>? _statusHandler;
    private EventHandler<WialonSessionEventArgs>? _expiredHandler;
    private EventHandler<WialonSessionEventArgs>? _tokenExpiringHandler;

    public WialonSessionBackgroundService(
        IWialonService sessionService,
        ILogger<WialonSessionBackgroundServicex> logger)
    {
        _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Wialon Session Background Service starting");

        // Setup event handlers with logging
        _statusHandler = (s, e) =>
        {
            _logger.LogInformation(
                "Session status changed: Active={IsActive}, ID={SessionId}, Message={Message}, Time={Timestamp}",
                e.IsActive, e.SessionId ?? "null", e.Message, e.Timestamp);
        };

        _expiredHandler = (s, e) =>
        {
            _logger.LogWarning("Session expired at {Timestamp}: {Message}", e.Timestamp, e.Message);
        };

        _tokenExpiringHandler = (s, e) =>
        {
            _logger.LogError("Token expiring at {Timestamp}: {Message}", e.Timestamp, e.Message);
            // Here you could add custom logic like:
            // - Send email notification to admin
            // - Log to monitoring system
            // - Trigger token refresh from secure storage
        };

        // Subscribe to service events
        _sessionService.SessionStatusChanged += _statusHandler;
        _sessionService.SessionExpired += _expiredHandler;
        _sessionService.TokenExpiring += _tokenExpiringHandler;

        try
        {
            // Ensure only one initialization attempt
            await _startupLock.WaitAsync(stoppingToken);
            try
            {
                if (!_isStarted)
                {
                    await InitializeSessionWithRetryAsync(stoppingToken);
                    _isStarted = true;
                }
            }
            finally
            {
                _startupLock.Release();
            }

            // Main monitoring loop
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                    
                    // Periodic health check
                    // if (!await _sessionService.IsSessionValidAsync())
                    // {
                    //     _logger.LogWarning("Session health check failed, attempting reconnect");
                    //     await _sessionService.EnsureSessionAsync(stoppingToken);
                    // }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in session health monitoring loop");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
        finally
        {
            // Cleanup event handlers
            if (_statusHandler != null)
                _sessionService.SessionStatusChanged -= _statusHandler;
            if (_expiredHandler != null)
                _sessionService.SessionExpired -= _expiredHandler;
            if (_tokenExpiringHandler != null)
                _sessionService.TokenExpiring -= _tokenExpiringHandler;
            
            _logger.LogInformation("Wialon Session Background Service stopping");
            
            // Attempt graceful logout
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
               // await _sessionService.LogoutAsync(cts.Token);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error during logout on service stop");
            }
        }
    }

    /// <summary>
    /// Initializes the Wialon session with retry logic
    /// </summary>
    private async Task InitializeSessionWithRetryAsync(CancellationToken stoppingToken)
    {
        const int maxRetries = 5;
        var retryCount = 0;
        var baseDelay = TimeSpan.FromSeconds(2);

        while (retryCount < maxRetries && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Initializing Wialon session (Attempt {Attempt}/{MaxRetries})", 
                    retryCount + 1, maxRetries);
                
                // var success = await _sessionService.EnsureSessionAsync(stoppingToken);
                // if (success)
                // {
                //     _logger.LogInformation("Wialon session initialized successfully");
                //     return;
                // }
                
                retryCount++;
                if (retryCount < maxRetries)
                {
                    // Exponential backoff
                    var delay = TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * Math.Pow(2, retryCount - 1));
                    _logger.LogWarning("Failed to initialize session, retrying in {DelaySeconds:F1} seconds", 
                        delay.TotalSeconds);
                    await Task.Delay(delay, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing Wialon session (Attempt {Attempt})", retryCount + 1);
                retryCount++;
                
                if (retryCount < maxRetries && !stoppingToken.IsCancellationRequested)
                {
                    var delay = TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * Math.Pow(2, retryCount - 1));
                    await Task.Delay(delay, stoppingToken);
                }
            }
        }

        if (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogError("Failed to initialize Wialon session after {MaxRetries} attempts", maxRetries);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Wialon Session Background Service is stopping");
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        if (!_disposed)
        {
            _startupLock?.Dispose();
            _disposed = true;
        }
        base.Dispose();
    }
/* 
    protected override void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _startupLock?.Dispose();
            _disposed = true;
        }
        base.Dispose(disposing);
    } */
}