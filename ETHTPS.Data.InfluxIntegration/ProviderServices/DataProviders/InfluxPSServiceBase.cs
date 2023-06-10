using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

namespace ETHTPS.Data.Integrations.InfluxIntegration.ProviderServices.DataProviders
{
    /// <summary>
    /// Represents a service that returns provider from InfluxDB data based on simple filters.
    /// </summary>
    /// <seealso cref="ETHTPS.API.BIL.Infrastructure.Services.DataServices.IPSService" />
    public abstract class InfluxPSServiceBase : IPSService
    {
        private readonly IInfluxWrapper _influxWrapper;
        private readonly Func<Block, double> _valueSelector;
        private readonly IRedisCacheService _redisCacheService;
        protected InfluxPSServiceBase(IInfluxWrapper influxWrapper, Func<Block, double> valueSelector, IRedisCacheService redisCacheService)
        {
            _influxWrapper = influxWrapper;
            _valueSelector = valueSelector;
            _redisCacheService = redisCacheService;
        }

        public async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetAsync(ProviderQueryModel model, TimeInterval interval)
        {
            var result = new Dictionary<string, List<DataResponseModel>>();
            await foreach (var entry in _influxWrapper.GetEntriesForPeriod<Block
                >(Constants.Influx.DEFAULT_BLOCK_BUCKET_NAME, "blockinfo", interval))
            {
                if (model.Provider != Constants.All && entry.Provider != model.Provider)
                {
                    continue;
                }
                if (!result.TryGetValue(entry.Provider, out var list))
                {
                    list = new List<DataResponseModel>();
                    result.Add(entry.Provider, list);
                }

                var dataPoint = new DataPoint()
                {
                    BlockNumber = entry.BlockNumber,
                    Date = entry.Date,
                    Value = _valueSelector(entry)
                };

                if (list.Count > 0)
                {
                    var lastDataPoint = list.Last().Data.Last();
                    var timeDiffInSeconds = (dataPoint.Date - lastDataPoint.Date).TotalSeconds;
                    if (timeDiffInSeconds > 0)
                    {
                        dataPoint.Value /= timeDiffInSeconds;
                    }
                    else
                    {
                        dataPoint.Value = 0;
                    }
                }

                list.Add(new DataResponseModel()
                {
                    Data = new List<DataPoint>() { dataPoint }
                });
            }

            return result.ToDictionary(x => x.Key, x => x.Value.AsEnumerable());
        }

        public Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetMonthlyDataByYearAsync(ProviderQueryModel model, int year)
        {
            throw new NotImplementedException();
        }


        public Task<IDictionary<string, IEnumerable<DataPoint>>> InstantAsync(ProviderQueryModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, DataPoint>> MaxAsync(ProviderQueryModel model)
        {
            throw new NotImplementedException();
        }
    }
}
