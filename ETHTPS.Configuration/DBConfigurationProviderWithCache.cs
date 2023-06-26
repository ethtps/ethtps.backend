using System.Collections.Concurrent;
using System.Reflection;

using ETHTPS.Configuration.Database;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Core;

using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ETHTPS.Configuration
{
    /// <summary>
    /// A wrapper around <see cref="DBConfigurationProvider"/> that caches the results of the DB calls for some time. The purpose of this class is to reduce the number of DB calls.
    /// </summary>
    public sealed class DBConfigurationProviderWithCache : IDBConfigurationProvider
    {
        private static readonly ConcurrentBag<string> _pendingAddOperations = new();
        private const int _TTL = 300;
        private readonly DBConfigurationProvider _provider;
        private readonly IRedisCacheService _redisCacheService;

        /// <summary>
        /// Gets the name of the application that instantiated this type initially. (should at least approximate the scope of the app instance...)
        /// </summary>
        public static string? EntryAppName { get; private set; }

        public DBConfigurationProviderWithCache(DBConfigurationProvider provider, IRedisCacheService redisCacheService)
        {
            _provider = provider;
            _redisCacheService = redisCacheService;
            EntryAppName ??= Assembly.GetEntryAssembly()?.GetName().Name?.Replace("ETHTPS.API", "ETHTPS.API.General");
        }

        private string GenerateKey(string partialKey) => $"Config:{EntryAppName}:{_provider._environment}:{partialKey}";

        /// <summary>
        /// Gets the value of the configuration entry with the specified name - from the cache, if it exists, else it uses the getter and caches the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="partialKey"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        /// <exception cref="AmbiguousActionException">Thrown when there is a concurrency issue</exception>
        /// <exception cref="InvalidOperationException">Attempted to return null object from cache. How did it even get there in the first place?</exception>
        private T Get<T>(string partialKey, Func<T> getter)
        {
            var key = GenerateKey(partialKey);
            while (_pendingAddOperations.Contains(key)) Thread.Sleep(50);
            if (!_redisCacheService.HasKey(key))
            {
                if ((typeof(T).IsAbstract && !typeof(T).IsClass) ||
                    (typeof(T).IsInterface && !typeof(T).IsIEnumerable()))
                {
                    if (typeof(T).IsIEnumerable() && typeof(T).GetGenericArguments().All(t => t is { IsClass: true, IsAbstract: false })) { }//ok
                    else
#if DEBUG
                        throw new InvalidOperationException(
                        $"Can't deserialize to an interface. Please use a concrete type instead of {typeof(T).GetGenericTypeDefinition().Name}<{string.Join(',', typeof(T).GetGenericArguments().Select(gt => gt.Name))}>.");
#else
                    throw new InvalidOperationException("Can't deserialize to an interface. Please use a concrete type.");
#endif
                }

                T result = getter();
                try
                {
                    _pendingAddOperations.Add(key);
                    _redisCacheService.SetData(key, result, TimeSpan.FromSeconds(_TTL));
                }
                finally
                {
                    if (!_pendingAddOperations.TryTake(out _)) throw new AmbiguousActionException("Couldn't remove object from concurrent bag. What should I do? :/");
                }

                return result;
            }
            else
            {
                return _redisCacheService.GetData<T>(key) ?? throw new InvalidOperationException("Attempt to return null object from cache");
            }
        }

        private void Set<T>(T value, string partialKey) => _redisCacheService.SetData(GenerateKey(partialKey), value, TimeSpan.FromSeconds(_TTL));

        public int? GetEnvironmentID(string name)
        {
            var key = $"{name}_ENVID";
            return Get(key, () => _provider.GetEnvironmentID(name));
        }

        public IEnumerable<IMicroservice>? GetMicroservices()
        {
            return Get("Microservices", () => _provider.GetMicroservices()?.Select(Microservice.From));
        }

        public void AddMicroservice(string name, string? description)
        {
            _provider.AddMicroservice(name, description);
        }

        public int? GetMicroserviceID(string name, bool addIfItDoesntExist)
        {
            var key = $"{name}_{addIfItDoesntExist}_MID";
            return Get(key, () => _provider.GetMicroserviceID(name, addIfItDoesntExist));
        }

        public IEnumerable<IConfigurationString>? GetConfigurationStringsForMicroservice(IMicroservice microservice)
        {
            var key = microservice.ToCacheKey();
            return Get(key, () => _provider.GetConfigurationStringsForMicroservice(microservice)?.Select(ConfigurationString.From));
        }

        public IEnumerable<IConfigurationString>? GetConfigurationStringsForMicroservice(string microserviceName)
        {
            var key = $"{microserviceName}_{_provider._environment}_ALL";
            return Get<IEnumerable<ConfigurationString>>(key, () => _provider.GetConfigurationStringsForMicroservice(microserviceName)?.Select(ConfigurationString.From) ?? Enumerable.Empty<ConfigurationString>()); // Conversion needed because we can't deserialize to an interface
        }

        public IEnumerable<string>? GetEnvironments()
        {
            return Get("Environments", () => _provider.GetEnvironments());
        }

        public IEnumerable<IConfigurationString> GetConfigurationStringsForProvider(string provider)
        {
            var key = $"{provider}_P";
            return Get(key, () => _provider.GetConfigurationStringsForProvider(provider)?.Select(ConfigurationString.From) ?? Enumerable.Empty<ConfigurationString>());
        }

        public IEnumerable<IConfigurationString>? GetConfigurationStrings(string name)
        {
            var key = $"{name}_GCS";
            return Get(key, () => _provider.GetConfigurationStrings(name)?.Select(ConfigurationString.From) ?? Enumerable.Empty<ConfigurationString>());
        }

        public IEnumerable<AllConfigurationStringsModel> GetAllConfigurationStrings()
        {
            return Get("ACS", () => _provider.GetAllConfigurationStrings());
        }

        public ConfigurationStringLinksModel GetAllLinks(int configurationStringID)
        {
            var key = $"{configurationStringID}_L_ALL";
            return Get(key, () => _provider.GetAllLinks(configurationStringID));
        }


        public void SetConfigurationStringForMicroservice(IMicroservice microservice, IConfigurationString configString)
        {
            _provider.SetConfigurationStringForMicroservice(microservice, configString);
        }

        public int? SetConfigurationString(IConfigurationString configString)
        {
            return _provider.SetConfigurationString(configString);
        }

        public void SetConfigurationStringForMicroservice(string microserviceName, IConfigurationString configString)
        {
            _provider.SetConfigurationStringForMicroservice(microserviceName, configString);
        }

        public void AddEnvironments(params string[] environments)
        {
            _provider.AddEnvironments(environments);
        }

        public void SetConfigurationStringsForProvider(string provider, params IConfigurationString[] configStrings)
        {
            _provider.SetConfigurationStringsForProvider(provider, configStrings);
        }

        public void Dispose()
        {
            _provider.Dispose();
        }

        public int AddOrUpdateConfigurationString(ConfigurationStringUpdateModel configurationString, string? microservice = null,
            string? environment = null)
        {
            return _provider.AddOrUpdateConfigurationString(configurationString, microservice, environment);
        }

        public int LinkProviderToConfigurationString(string providerName, string configurationStringName,
            string environmentName = Constants.ENVIRONMENT)
        {
            return _provider.LinkProviderToConfigurationString(providerName, configurationStringName, environmentName);
        }

        public int LinkProviderToConfigurationString(int providerID, int configurationStringID,
            string environmentName = Constants.ENVIRONMENT)
        {
            return _provider.LinkProviderToConfigurationString(providerID, configurationStringID, environmentName);
        }

        public int UnlinkProviderFromConfigurationString(int providerID, int configurationStringID,
            string environmentName = Constants.ENVIRONMENT)
        {
            return _provider.UnlinkProviderFromConfigurationString(providerID, configurationStringID, environmentName);
        }

        public int ClearHangfireQueue()
        {
            return _provider.ClearHangfireQueue();
        }
    }
}
