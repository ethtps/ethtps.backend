using System.Reflection;

using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.API.BIL.Infrastructure.Services.BlockInfo
{
    public static class BlockInfoProviderDiscoverer
    {
        private static List<Type> GetBlockInfoProviders(string dllName)
        {
            Type interfaceType = typeof(IHTTPBlockInfoProvider);
            List<Type> concreteTypes = Assembly.LoadFrom(dllName).GetTypes()
                .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToList();
            return concreteTypes;
        }

        public static IServiceCollection AddAutoDiscoveredIHTTPBlockInfoProvidersToDatabase(this IServiceCollection services)
        {
            var providers = GetBlockInfoProviders(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "ETHTPS.Services.Ethereum.dll"));
            if (providers != null && providers.Any())
            {
                using (var provider = services.BuildServiceProvider())
                {
                    var context = provider.GetRequiredService<EthtpsContext>();
                    var statusService = provider.GetRequiredService<IDataUpdaterStatusService>();
                    foreach (var p in providers)
                    {
                        // BlockInfo means both historical and TPSGPS
                        statusService.SetStatusFor(p.GetProviderName(), Data.Core.Models.DataUpdater.UpdaterType.BlockInfo, Data.Core.Models.DataUpdater.UpdaterStatus.Idle);
                        //statusService.SetStatusFor(p.GetProviderName(), Data.Core.Models.DataUpdater.UpdaterType.Historical, Data.Core.Models.DataUpdater.UpdaterStatus.Idle);
                        //statusService.SetStatusFor(p.GetProviderName(), Data.Core.Models.DataUpdater.UpdaterType.TPSGPS, Data.Core.Models.DataUpdater.UpdaterStatus.Idle);
                    }
                }
            }
            return services;
        }
    }
}
