using System.Collections.Concurrent;

using ETHTPS.Data.Core;

using Microsoft.AspNetCore.SignalR;

namespace ETHTPS.WSAPI.Infrastructure.LiveData.Connection
{
    public sealed class LiveDataHub : Hub
    {
        private static ILogger<LiveDataHub>? _logger;

        public LiveDataHub(ILogger<LiveDataHub> logger)
        {
            _logger = logger;
        }

        private static readonly ConcurrentDictionary<string, SubscriptionModel> _subscriptions = new();
        private string _lastBroadcast = string.Empty;

        public override async Task OnConnectedAsync()
        {
            await SubscribeAsync(new SubscriptionModel()
            {
                DataTypes = new[] { DataType.TPS, DataType.GasAdjustedTPS, DataType.GPS },
                IncludeSidechains = true,
                IncludeTransactions = true
            });
            await base.OnConnectedAsync();
        }

        public async Task SubscribeAsync(SubscriptionModel subscription)
        {
            if (Context == null || Context.ConnectionId == null)
                return;

            _subscriptions.AddOrUpdate(Context.ConnectionId, subscription, (k, v) => v);
            await Clients.Client(Context.ConnectionId).SendAsync("ConnectionEstablished");
            _logger?.LogInformation($"New connection: {Context.ConnectionId}");
        }

        public async Task UnsubscribeAsync() => await UnsubscribeAsync(Context.ConnectionId);

        public static Task UnsubscribeAsync(string connectionID)
        {
            if (_subscriptions.ContainsKey(connectionID))
            {
                _logger?.LogInformation($"Terminating connection {connectionID} after {_subscriptions[connectionID].TimeAlive}");
                _subscriptions.Remove(connectionID, out _);
            }
            return Task.CompletedTask;
        }
    }
}
