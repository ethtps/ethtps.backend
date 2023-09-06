using System.Linq.Expressions;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Database;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.RPC;
using ETHTPS.Services.BlockchainServices.HangfireLogging;

using Microsoft.Extensions.Logging;

using static ETHTPS.Utils.Logging.LoggingUtils;

using Provider = ETHTPS.Data.Integrations.MSSQL.Provider;
using ProviderTag = ETHTPS.Data.Integrations.MSSQL.ProviderTag;
using Tag = ETHTPS.Data.Integrations.MSSQL.Tag;

namespace ETHTPS.Services.BackgroundTasks.Recurring.AutoDiscovery
{
    /// <summary>
    /// Checks for new EVM-compatible chains, adds them to the database in pending approval status and creates configurations in order to make them ready for data collection. In case the chain already exists, associated metadata will be updated.
    /// </summary>
    [RunsEvery(CronConstants.EVERY_HOUR)]
    public class RPCAutoDiscoveryTask : HangfireBackgroundService
    {
        private readonly ConfigurationContext _configurationContext;
        private readonly DBConfigurationProviderWithCache _configurationProvider;
        public RPCAutoDiscoveryTask(ILogger<HangfireBackgroundService> logger, EthtpsContext context, ConfigurationContext configurationContext, DBConfigurationProviderWithCache configuration) : base(logger, context)
        {
            _configurationContext = configurationContext;
            _configurationProvider = configuration;
        }

        protected override string ServiceName => "RPC auto-discovery";

        public int GetOrCreateEntityId<T>(Expression<Func<T, bool>> predicate, T defaultEntity) where T : class
        {
            var entity = _context.Set<T>().FirstOrDefault(predicate);
            if (entity == null)
            {
                _context.Set<T>().Add(defaultEntity);
                _context.SaveChanges();
#pragma warning disable CS8605 // Unboxing a possibly null value.
                return (int)typeof(T).GetProperty("Id")?.GetValue(defaultEntity);
            }
            return (int)typeof(T).GetProperty("Id")?.GetValue(entity);
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }


        // Horrible mess of a code I know
        public override async Task RunAsync()
        {
            Trace("Running RPC auto-discovery...");
            Trace("Getting data...");
            var chains = await ChainUtility.GenerateChainDataAsync();
            Trace("Got data");
            lock (_context.LockObj)
            {
                try
                {
                    var defaultTypeID = GetOrCreateEntityId(
                        x => x.Name == "EVM-compatible",
                        new ProviderType { Name = "EVM-compatible", Color = "#000000", IsGeneralPurpose = true, Enabled = true }
                    );
                    Trace($"EVM-compatible type ID: {defaultTypeID}");

                    var autoAddedTagID = GetOrCreateEntityId(
                        x => x.Name == "Auto-added",
                        new Tag { Name = "Auto-added" }
                    );

                    var unknownNetworkID = GetOrCreateEntityId(x => x.Name == "Unknown", new Network()
                    {
                        Name = "Unknown"
                    });

                    if (chains.Any())
                    {
                        Trace($"Got data for {chains.Count()} chains");
                        Trace("Saving data...");

                        foreach (var chain in chains)
                        {
                            Trace($"Checking {chain.Name}...");
                            var providerID = GetOrCreateEntityId(
                                x => x.Name == chain.Name,
                                new Provider { Enabled = true, PendingApproval = true, Name = chain.Name, Color = "#000000", TheoreticalMaxTps = 0, Type = defaultTypeID }
                            );
                            Trace($"{chain.Name} ID: {providerID}");

                            var providerTagID = GetOrCreateEntityId(
                                x => x.TagId == autoAddedTagID && x.ProviderId == providerID,
                                new ProviderTag { ProviderId = providerID, TagId = autoAddedTagID }
                            );
                            Trace($"{chain.Name} tag ID: {providerTagID}");

                            var rpcUpdaterID = GetOrCreateEntityId(x => x.Provider == providerID && x.Network == unknownNetworkID, new Updater()
                            {
                                Network = unknownNetworkID,
                                Provider = providerID,
                                LastUpdated = DateTime.UtcNow
                            });
                            Trace($"{chain.Name} RPC updater ID: {rpcUpdaterID}");

                            var rpcUpdaterConfigurationID = GetOrCreateEntityId(x => x.Updater == rpcUpdaterID,
                                new UpdaterConfiguration()
                                {
                                    Updater = rpcUpdaterID,
                                    Enabled = true,
                                    UpdateIntervalMs = 60,
                                    MaxRetries = 10,
                                    RetryIntervalMs = 10000,
                                });

                            Trace($"{chain.Name} RPC updater configuration ID: {rpcUpdaterConfigurationID}");

                            foreach (var rpcEndpoint in chain.Rpc)
                            {
                                var endpointID = GetOrCreateEntityId(x => x.Address == rpcEndpoint, new Endpoint()
                                {
                                    Address = rpcEndpoint,
                                    Description = "Auto-added"
                                });
                                Trace($"{chain.Name} RPC endpoint ID: {endpointID}");
                                var bindingID = GetOrCreateEntityId(x => x.Endpoint == endpointID && x.Updater == rpcUpdaterID, new Binding()
                                {
                                    Endpoint = endpointID,
                                    Updater = rpcUpdaterID,
                                    IsActive = true,
                                });
                                Trace($"{chain.Name} RPC binding ID: {bindingID}");
                            }
                        }
                    }
                    else
                    {
                        Trace("No data retrieved :/");
                    }
                }
                catch
                {
                    throw;
                }

            }
        }
    }
}
