﻿using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.TimeBuckets;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Integrations.InfluxIntegration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.TimeBuckets.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void InjectTimeBucketService<V>(this IServiceCollection services, DatabaseProvider databaseProvider)
           where V : class, IHTTPBlockInfoProvider
        {
            services.TryAddScoped<V>();
            if (databaseProvider == DatabaseProvider.InfluxDB)
            {
                services.AddScoped<ITimeBucketDataUpdaterService<V>, InfluxTimeBucketService<V>>();
            }
            else
            {
            }
        }

        public static void InjectTimeBucketService(this IServiceCollection serviceCollection, DatabaseProvider databaseProvider)
        {
            if (databaseProvider == DatabaseProvider.InfluxDB)
            {
                serviceCollection.AddScoped(typeof(ITimeBucketDataUpdaterService<>), typeof(InfluxTimeBucketService<>));
            }
        }
    }
}
