using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace CleanArchitecture.Blazor.Server.UI.Hubs;

public class ServerHubWrapper : IApplicationHubWrapper
{
    private readonly IHubContext<ServerHub, ISignalRHub> _hubContext;

    public ServerHubWrapper(IHubContext<ServerHub, ISignalRHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task JobStarted(int id, string message)
    {
        await _hubContext.Clients.All.Start(id,message).ConfigureAwait(false);
    }

    public async Task JobCompleted(int id, string message)
    {
        await _hubContext.Clients.All.Completed(id,message).ConfigureAwait(false); 
    }


// ── NEW ──
    public async Task SmsReceived(SmsReceivedPayload payload)
        => await _hubContext.Clients.All.SmsReceived(payload).ConfigureAwait(false);

    public async Task SmsStatusChanged(SmsStatusChangedPayload payload)
        => await _hubContext.Clients.All.SmsStatusChanged(payload).ConfigureAwait(false);
        
}
