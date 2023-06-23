using Coravel.Invocable;

namespace ETHTPS.WSAPI.Infrastructure.LiveData
{
    public sealed class DataUpdateQueue : IInvocable
    {
        public Task Invoke()
        {
            return Task.CompletedTask;
        }
    }
}
