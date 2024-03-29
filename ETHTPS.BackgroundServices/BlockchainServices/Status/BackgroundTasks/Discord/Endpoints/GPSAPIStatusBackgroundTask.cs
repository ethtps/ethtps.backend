﻿using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Services.BlockchainServices.HangfireLogging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ETHTPS.Services.BlockchainServices.Status.BackgroundTasks.Discord.Endpoints
{
    public sealed class GPSAPIStatusBackgroundTask : URLMonitoringBackgroundTask
    {
        public GPSAPIStatusBackgroundTask(ILogger<HangfireBackgroundService> logger, EthtpsContext context, IConfiguration configuration) : base(logger, context, configuration, "https://api.ethtps.info/API/GPS/Get")
        {
        }
    }
}
