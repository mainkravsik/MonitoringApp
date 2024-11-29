using Microsoft.AspNetCore.SignalR;
using MonitoringApp.Models;

namespace MonitoringApp.Hubs;

public class MonitoringHub : Hub
{
    public async Task NotifyStatus(ServiceStatus status)
    {
        await Clients.All.SendAsync("ReceiveStatus", status);
    }
}
