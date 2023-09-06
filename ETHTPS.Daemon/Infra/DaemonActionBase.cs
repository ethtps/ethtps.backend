using ETHTPS.Core;
using ETHTPS.Services.Infrastructure.Messaging;

namespace ETHTPS.Daemon.Infra
{
    public abstract class DaemonActionBase<T> : IDaemonAction<T>
    {
        protected readonly T? _object;
        protected readonly string _requestKey;
        protected readonly IMessagePublisher _publisher;
        protected readonly IRedisCacheService _cacheService;

        protected DaemonActionBase(string requestKey, IMessagePublisher publisher, IRedisCacheService cacheService, T? @object)
        {
            ArgumentException.ThrowIfNullOrEmpty(requestKey);
            ArgumentNullException.ThrowIfNull(publisher);
            ArgumentNullException.ThrowIfNull(cacheService);

            _requestKey = requestKey;
            _publisher = publisher;
            _cacheService = cacheService;

            if (@object == null)
            {
                if (!_cacheService.HasKey(_requestKey))
                {
                    throw new ArgumentException($"The request key {_requestKey} does not exist in the cache.");
                }
                var o = _cacheService.GetData<T>(_requestKey);
                ArgumentNullException.ThrowIfNull(_object);
                _object = o;
            }
        }

        public abstract Task RunAsync();
    }
}
