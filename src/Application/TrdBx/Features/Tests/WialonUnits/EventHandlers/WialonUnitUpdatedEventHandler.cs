// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.WialonUnits.EventHandlers;

public class WialonUnitUpdatedEventHandler : INotificationHandler<WialonUnitUpdatedEvent>
{
    private readonly ILogger<WialonUnitUpdatedEventHandler> _logger;

    public WialonUnitUpdatedEventHandler(
        ILogger<WialonUnitUpdatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(WialonUnitUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
