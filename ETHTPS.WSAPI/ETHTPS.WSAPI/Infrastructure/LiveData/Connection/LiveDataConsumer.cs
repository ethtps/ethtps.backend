using Microsoft.AspNetCore.SignalR.Client;

namespace ETHTPS.WSAPI.Infrastructure.LiveData.Connection
{
    public sealed class LiveDataConsumer : IAsyncDisposable
    {
        private HubConnection? _hubConnection;

        public LiveDataConsumer()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .Build();
        }

        public Task StartNotificationConnectionAsync() => Task.FromResult(_hubConnection?.StartAsync());

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
            }
        }
    }
}
