// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.WialonUnits.EventHandlers;

public class WialonUnitDeletedEventHandler : INotificationHandler<WialonUnitDeletedEvent>
{
    private readonly ILogger<WialonUnitDeletedEventHandler> _logger;

    public WialonUnitDeletedEventHandler(
        ILogger<WialonUnitDeletedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(WialonUnitDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}