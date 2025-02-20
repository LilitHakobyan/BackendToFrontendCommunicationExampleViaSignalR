using BackendToFrontendComunicationExample.Server.Models;
using BackendToFrontendComunicationExample.Server.SignalRConfigurations;
using Microsoft.AspNetCore.SignalR;

namespace BackendToFrontendComunicationExample.Server.Services;

public class EventBackgroundService : BackgroundService
{
    private readonly IHubContext<EntryEventHub> _hubContext;

    public EventBackgroundService(IHubContext<EntryEventHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            for (int i = 0; i < 5; i++)
            {
                var entry = new Entry
                {
                    Id = i + 15,
                    Name = "Entry name" + i,
                    StarTime = DateTime.Now.AddDays(i * i),
                };

                await Task.Delay(3000, stoppingToken); // Simulate event occurrence

                await _hubContext.Clients.All.SendAsync("ReceiveUpdate", entry);
            }
        }
    }
}