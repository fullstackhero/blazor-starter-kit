using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Hubs
{
    public class RealtimeHub : Hub
    {
        public async Task UpdateDashboardAsync()
        {
            await Clients.All.SendAsync("UpdateDashboard");
        }
    }
}
