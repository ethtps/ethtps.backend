using Microsoft.AspNetCore.SignalR;
using ETHTPS.Data.Core.Models.LiveData.Triggers;
using ETHTPS.Data.Core;

namespace ETHTPS.WSAPI.Infrastructure.LiveData.Connection
{
    public class LiveDataHub : Hub
    {
        private static readonly Dictionary<string, SubscriptionModel> _subscriptions = new Dictionary<string, SubscriptionModel>();

        public LiveDataHub()
        {
            Task.Run(async () => await SubscribeAsync(new SubscriptionModel()
            {
                DataTypes = new[] {DataType.TPS, DataType.GasAdjustedTPS, DataType.GPS},
                IncludeSidechains = true,
                IncludeTransactions = true,
            })).Wait();
        }

        public async Task SubscribeAsync(SubscriptionModel subscription)
        {
            if (Context == null || Context.ConnectionId == null)
                return;

            if (!_subscriptions.ContainsKey(Context.ConnectionId))
            {
                _subscriptions.Add(Context.ConnectionId, subscription);
                await Clients.Client(Context.ConnectionId).SendAsync("ConnectionEstablished");
                Console.WriteLine($"New connection: {Context.ConnectionId}");
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
                Console.WriteLine($"Terminating connection {connectionID} after {_subscriptions[connectionID].TimeAlive}");
                _subscriptions.Remove(connectionID);
            }
            return Task.CompletedTask;
        }

        public async Task SendNotificationsToEveryoneAsync() => await Clients.All.SendAsync("DataUpdate");
        
    }
}
