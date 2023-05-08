﻿using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.TimeBuckets;
using ETHTPS.Data.Core.BlockInfo;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.TimeBuckets.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void InjectTimeBucketService<V>(this IServiceCollection services)
           where V : class, IHTTPBlockInfoProvider
        {
            services.TryAddScoped<V>();
            services.AddScoped<ITimeBucketDataUpdaterService<V>, MSSQLTimeBucketService<V>>();
        }

        public static void InjectTimeBucketService(this IServiceCollection serviceCollection) => serviceCollection.AddScoped(typeof(ITimeBucketDataUpdaterService<>), typeof(MSSQLTimeBucketService<>));
    }
}
