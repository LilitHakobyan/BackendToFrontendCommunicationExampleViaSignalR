using BackendToFrontendComunicationExample.Server.Models;
using Microsoft.AspNetCore.SignalR;

namespace BackendToFrontendComunicationExample.Server.SignalRConfigurations;

public interface IEntryEventClient
{
    Task ReceiveEntryEvent(string eventType, Entry entry);
    Task ConnectionEst(string status);
}

public class EntryEventHub : Hub<IEntryEventClient>
{
    public async Task SendEntryEvent(Entry updatedObject)
    {
        await Clients.Caller.ReceiveEntryEvent("update", updatedObject);
    }

    public override async Task OnConnectedAsync()
    {
         await Clients.Caller.ConnectionEst("status");
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Clients.Caller.ConnectionEst("status");
    }
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}