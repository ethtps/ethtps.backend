﻿using Microsoft.AspNetCore.SignalR;

namespace ETHTPS.WSAPI.Infrastructure.LiveData.Connection
{
    public sealed class LiveDataService
    {
        private readonly IHubContext<LiveDataHub> _hubContext;

        public LiveDataService(IHubContext<LiveDataHub> hubContext) =>
            _hubContext = hubContext;

        public Task SendNotificationAsync()
        {
            return _hubContext.Clients.All.SendAsync("NotificationReceived");
        }
    }
}
