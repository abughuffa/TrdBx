// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.EventHandlers;

public class WialonUnitCreatedEventHandler : INotificationHandler<WialonUnitCreatedEvent>
{
    private readonly ILogger<WialonUnitCreatedEventHandler> _logger;

    public WialonUnitCreatedEventHandler(
        ILogger<WialonUnitCreatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(WialonUnitCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
