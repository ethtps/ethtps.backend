using Coravel.Events.Interfaces;

using ETHTPS.API.Core.Services.LiveData;
using ETHTPS.Data.Core;

using Microsoft.AspNetCore.SignalR;

using Newtonsoft.Json;

namespace ETHTPS.WSAPI.Infrastructure.LiveData.Connection
{
    public sealed class LiveDataHub : Hub, IListener<LiveDataChanged>
    {
        private static ILogger<LiveDataHub>? _logger;

        public LiveDataHub(ILogger<LiveDataHub> logger)
        {
            _logger = logger;
        }

        private static readonly Dictionary<string, SubscriptionModel> _subscriptions = new Dictionary<string, SubscriptionModel>();
        private LiveDataChanged? _lastBroadcast;


        public override async Task OnConnectedAsync()
        {
            await SubscribeAsync(new SubscriptionModel()
            {
                DataTypes = new[] { DataType.TPS, DataType.GasAdjustedTPS, DataType.GPS },
                IncludeSidechains = true,
                IncludeTransactions = true,
            });
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await UnsubscribeAsync();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SubscribeAsync(SubscriptionModel subscription)
        {
            if (Context == null || Context.ConnectionId == null)
                return;

            if (!_subscriptions.ContainsKey(Context.ConnectionId))
            {
                _subscriptions.Add(Context.ConnectionId, subscription);
                await Clients.Client(Context.ConnectionId).SendAsync("ConnectionEstablished");
                _logger?.LogInformation($"New connection: {Context.ConnectionId}");
            }
            else
            {
                _subscriptions[Context.ConnectionId] = subscription;
            }
        }

        public async Task UnsubscribeAsync() => await UnsubscribeAsync(Context.ConnectionId);

        public static Task UnsubscribeAsync(string connectionID)
        {
            if (_subscriptions.ContainsKey(connectionID))
            {
                _logger?.LogInformation($"Terminating connection {connectionID} after {_subscriptions[connectionID].TimeAlive}");
                _subscriptions.Remove(connectionID);
            }
            return Task.CompletedTask;
        }

        public async Task SendNotificationsToEveryoneAsync()
        {
            if (_lastBroadcast != null && Clients?.All != null)
            {
                await Clients.All.SendAsync(JsonConvert.SerializeObject(_lastBroadcast));
            }
        }

        public async Task HandleAsync(LiveDataChanged broadcasted)
        {
            if (broadcasted == null)
                return;

            _lastBroadcast = broadcasted;
            await SendNotificationsToEveryoneAsync();
        }
    }
}
