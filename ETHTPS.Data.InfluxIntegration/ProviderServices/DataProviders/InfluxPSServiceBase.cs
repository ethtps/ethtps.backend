using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Attributes;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.LiveData;
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
        private readonly Func<MinimalDataPoint, double?> _datapointValueSelector;
        private readonly IRedisCacheService _redisCacheService;

        protected InfluxPSServiceBase(
            IInfluxWrapper influxWrapper,
            Func<Block, double> valueSelector,
            Func<MinimalDataPoint, double?> datapointValueSelector,
            IRedisCacheService redisCacheService)
        {
            _influxWrapper = influxWrapper;
            _valueSelector = valueSelector;
            _redisCacheService = redisCacheService;
            _datapointValueSelector = datapointValueSelector;
        }

        public async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetAsync(ProviderQueryModel model, TimeInterval interval)
        {
            var result = new Dictionary<string, List<DataResponseModel>>();
            await foreach (var entry in
                (model.Provider == Constants.All ?
                _influxWrapper.GetEntriesForPeriod<Block
                >(Constants.Influx.DEFAULT_BLOCK_BUCKET_NAME, "blockinfo", interval) :
                _influxWrapper.GetEntriesForPeriod<Block
                >(Constants.Influx.DEFAULT_BLOCK_BUCKET_NAME, "blockinfo", model.Provider, interval)))
            {
                if (model.Provider != Constants.All && entry.Provider != model.Provider)
                {
                    continue; //Redundant(?)
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

        public async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetMonthlyDataByYearAsync(ProviderQueryModel model, int year)
        {
            var result = new Dictionary<string, List<DataResponseModel>>();
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);
            await foreach (var entry in _influxWrapper.GetEntriesBetween<Block
                >(Constants.Influx.DEFAULT_BLOCK_BUCKET_NAME, "blockinfo", start, end, "1mo"))
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


        public async Task<IDictionary<string, IEnumerable<DataPoint>>> InstantAsync(ProviderQueryModel model)
        {
            if (model.Provider != Constants.All)
            {
                var data = await _redisCacheService.GetDataAsync<MinimalDataPoint>($"Instant_{model.Provider}");
                if (data == null)
                    return new Dictionary<string, IEnumerable<DataPoint>>();

                return new Dictionary<string, IEnumerable<DataPoint>>()
                {
                    {
                        model.Provider, new DataPoint[]{ new DataPoint()
                    {
                        Value = _datapointValueSelector(data)??0
                    } }
                    }
                };
            }
            throw new NotSupportedException("Operation not allowed: Get max for ALL providers");
        }

        public async Task<IDictionary<string, DataPoint>> MaxAsync(ProviderQueryModel model)
        {
            if (model.Provider != Constants.All)
            {
                var data = await _redisCacheService.GetDataAsync<MinimalDataPoint>($"Max_{model.Provider}");
                if (data == null)
                    return new Dictionary<string, DataPoint>();

                return new Dictionary<string, DataPoint>()
                {
                    {
                        model.Provider, new DataPoint()
                    {
                        Value = _datapointValueSelector(data)??0
                    }
                    }
                };
            }
            throw new NotSupportedException("Operation not allowed: Get max for ALL providers");
        }

        public async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetAsync(L2DataRequestModel model)
        {
            var result = new Dictionary<string, List<DataResponseModel>>();
            await foreach (var entry in
                _influxWrapper
                .GetEntriesBetween<Block>(
                    Constants.Influx.DEFAULT_BLOCK_BUCKET_NAME,
                    "blockinfo",
                    model.StartDate ?? new DateTime(),
                    model.EndDate ?? new DateTime(),
                "1" + model.AutoInterval.ExtractTimeGrouping().ToTimeSpan().ToFluxTimeUnit()))
            {
                if (model.Provider != Constants.All && !model.AllDistinctProviders.Contains(model.Provider))
                {
                    continue; //Redundant(?)
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
    }
}
