using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Integrations.InfluxIntegration;

using Microsoft.Extensions.Logging;

namespace ETHTPS.Services.BlockchainServices.CoravelLoggers.AZ
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class AzureBlockLogger<T> : CoravelBlockLogger<T>
    where T : IHTTPBlockInfoProvider
    {
        public AzureBlockLogger(ILogger<CoravelBlockLogger<T>> logger, IDataUpdaterStatusService statusService, T instance, IInfluxWrapper influxWrapper) : base(logger, statusService, instance, influxWrapper)
        {
        }
    }
}