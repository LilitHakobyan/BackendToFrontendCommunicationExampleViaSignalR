using Microsoft.AspNetCore.SignalR;

namespace BackendToFrontendComunicationExample.Server.SignalRConfigurations;

public class EntryEventHub : Hub
{
    public async Task SendUpdate(object updatedObject)
    {
        await Clients.All.SendAsync("ReceiveUpdate", updatedObject);
    }
}