using CleanArchitecture.Blazor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;

namespace CleanArchitecture.Blazor.Server.UI.Hubs;

public interface ISignalRHub
{
    public const string Url = "/signalRHub";

    Task Connect(string connectionId, string userName);
    Task Disconnect(string connectionId, string userName);

    Task Start(int id, string message);
    Task Completed(int id, string message);

    Task SendMessage(string from, string message);
    Task SendPrivateMessage(string from, string to, string message);
    Task SendNotification(string message);

    Task PageComponentOpened(string pageComponent, string userId, string userName);
    Task PageComponentClosed(string pageComponent, string userId, string userName);

    Task<List<UserContext>> GetOnlineUsers();

    // ── NEW: SMS events ────────────────────────────────────────────────
    /// <summary>Fired when an inbound SMS is stored (webhook or poller path).</summary>
    Task SmsReceived(SmsReceivedPayload payload);

    /// <summary>Fired when an outbound SMS changes state (queued→sent→delivered/failed).</summary>
    Task SmsStatusChanged(SmsStatusChangedPayload payload);
}

