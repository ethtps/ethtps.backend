using ETHTPS.API.BIL.Infrastructure.Services.DataServices;
using ETHTPS.Core;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.LiveData;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.MSSQL;

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
        private readonly string[] _allAvailableProviders;

        protected InfluxPSServiceBase(
            IInfluxWrapper influxWrapper,
            Func<Block, double> valueSelector,
            Func<MinimalDataPoint, double?> datapointValueSelector,
            IRedisCacheService redisCacheService,
            EthtpsContext context)
        {
            _influxWrapper = influxWrapper;
            _valueSelector = valueSelector;
            _redisCacheService = redisCacheService;
            _datapointValueSelector = datapointValueSelector;
            _allAvailableProviders = context.Providers.Select(x => x.Name).ToArray();
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
                entry.Provider = model.Provider;
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
            //if (model.Provider == Constants.All) throw new NotSupportedException("InfluxDB doesn\'t return strings for provider names - TOFIX");
            var result = new Dictionary<string, List<DataResponseModel>>();
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);
            await foreach (var entry in _influxWrapper.GetEntriesBetween<Block
                >(Constants.Influx.DEFAULT_BLOCK_BUCKET_NAME, "blockinfo", start, end, "1mo"))
            {
                entry.Provider = model.Provider;
                if (model.Provider != Constants.All && entry.Provider != model.Provider)
                {
                    continue;
                }
                if (!result.TryGetValue(entry.Provider, out var list))
                {
                    list = new List<DataResponseModel>();
                    result[entry.Provider] = list;
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
            else
            {
                var result = new Dictionary<string, IEnumerable<DataPoint>>();
                foreach (var provider in _allAvailableProviders)
                {
                    var x = await InstantAsync(new ProviderQueryModel()
                    {
                        Provider = provider,
                        IncludeSidechains = model.IncludeSidechains,
                        Network = model.Network
                    });
                    if (x.Count > 0)
                    {
                        result.Add(x.First().Key, x.First().Value);
                    }
                }
                return result;
            }
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
            else
            {
                var result = new Dictionary<string, DataPoint>();
                foreach (var provider in _allAvailableProviders)
                {
                    var x = await MaxAsync(new ProviderQueryModel()
                    {
                        Provider = provider,
                        IncludeSidechains = model.IncludeSidechains,
                        Network = model.Network
                    });
                    if (x.Count > 0)
                    {
                        result.Add(x.First().Key, x.First().Value);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Replaces the provider name in the list of blocks with the given provider name. Useful because InfluxDB returns numbers back instead of names.
        /// </summary>
        private static IEnumerable<InfluxBlock> SetProvider(IEnumerable<InfluxBlock> list, string provider)
        {
            foreach (var item in list)
            {
                item.Provider = provider;
                yield return item;
            }
        }

        public async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetAsync(L2DataRequestModel model)
        {
            var result = new Dictionary<string, List<DataResponseModel>>();
            model.StartDate ??= DateTime.Now.Subtract(TimeSpan.FromDays(365 * 10 + 2)); // Default to 10 years ago
            model.EndDate ??= DateTime.Now; // Default to now

            var providerData = new List<IEnumerable<InfluxBlock>>();

            if (string.IsNullOrWhiteSpace(model.Provider) || model.Provider == Constants.All)
            {
                foreach (var provider in model.AllDistinctProviders)
                {
                    providerData.Add(SetProvider(await _influxWrapper
                    .GetEntriesBetweenAsync<InfluxBlock>(
                    Constants.Influx.DEFAULT_BLOCK_BUCKET_NAME,
                    "blockinfo",
                    provider,
                    model.StartDate.Value,
                    model.EndDate.Value), provider));
                }
            }
            else
            {
                providerData.Add(SetProvider(await _influxWrapper
                .GetEntriesBetweenAsync<InfluxBlock>(
                    Constants.Influx.DEFAULT_BLOCK_BUCKET_NAME,
                    "blockinfo",
                    model.Provider,
                    model.StartDate.Value,
                    model.EndDate.Value), model.Provider));
            }

            foreach (var data in providerData)
            {
                if (!data.Any())
                {
                    continue;
                }

                var provider = data.First().Provider;
                if (!result.TryGetValue(provider, out var list))
                {
                    list = new List<DataResponseModel>();
                    result[provider] = list;
                }

                var dataResponse = new DataResponseModel()
                {
                    Data = new List<DataPoint>()
                };

                foreach (var entry in data)
                {
                    var dataPoint = new DataPoint()
                    {
                        BlockNumber = (int)entry.BlockNumber,
                        Date = entry.Date,
                        Value = _valueSelector(entry.ToBlock())
                    };

                    if (dataResponse.Data.Count > 0)
                    {
                        var lastDataPoint = dataResponse.Data.Last();
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

                    dataResponse.Data.Add(dataPoint);
                }

                list.Add(dataResponse);
            }


            return result.ToDictionary(x => x.Key, x => x.Value.AsEnumerable());
        }

    }
}
