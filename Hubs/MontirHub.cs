using Microsoft.AspNetCore.SignalR;
using TelemedicinaMonitores.Models;

namespace TelemedicinaMonitores.Hubs

{
    public class MonitorHub : Hub
    {
        public async Task SendMonitorData(string monitorId, PatientData data)
        {
            await Clients.All.SendAsync("ReceiveMonitorData", monitorId, data);
        }

        public async Task JoinMonitorGroup(string monitorId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, monitorId);
        }
    }
}