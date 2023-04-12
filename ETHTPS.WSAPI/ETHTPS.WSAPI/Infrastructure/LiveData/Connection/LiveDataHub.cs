using Microsoft.AspNetCore.SignalR;
using ETHTPS.Data.Core.Models.LiveData.Triggers;

namespace ETHTPS.WSAPI.Infrastructure.LiveData.Connection
{
    public class LiveDataHub : Hub
    {
        public LiveDataHub()
        {
            Console.WriteLine(  "Live data hub up");
        }
        private static readonly Dictionary<string, SubscriptionModel> _subscriptions = new Dictionary<string, SubscriptionModel>();

        public Task SubscribeAsync(SubscriptionModel subscription)
        {
            if (!_subscriptions.ContainsKey(Context.ConnectionId))
            {
                _subscriptions.Add(Context.ConnectionId, subscription);
            }
            else
            {
                _subscriptions[Context.ConnectionId] = subscription;
            }
            return Task.CompletedTask;
        }

        public async Task UnsubscribeAsync() => await UnsubscribeAsync(Context.ConnectionId);

        public static Task UnsubscribeAsync(string connectionID)
        {
            if (_subscriptions.ContainsKey(connectionID))
            {
                _subscriptions.Remove(connectionID);
            }
            return Task.CompletedTask;
        }

        public async Task SendNotificationsToEveryoneAsync() => await Clients.All.SendAsync("DataUpdate");
        
    }
}
