using BackendToFrontendComunicationExample.Server.Models;
using Microsoft.AspNetCore.SignalR;
using System;

namespace BackendToFrontendComunicationExample.Server.SignalRConfigurations;

public class EntryEventService
{
    private readonly IHubContext<EntryEventHub> _hubContext;

    public EntryEventService(IHubContext<EntryEventHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(Entry updatedObject)
    {
        for (int i = 0; i < 5; i++)
        {
            var entry = new Entry
            {
                Id = i + 15,
                Name = "Entry name" + i,
                StarTime = DateTime.Now.AddDays(i * i),
            };

            await Task.Delay(3000); // Simulate processing time

           
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", entry);
        }

    }
}