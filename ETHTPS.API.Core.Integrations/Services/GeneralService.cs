using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GTPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.TPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.Core.Services;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Extensions.StringExtensions;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Core.Models.ResponseModels;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.HistoricalDataProviders;

using ServiceStack;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services
{
    public sealed class GeneralService : HistoricalMethodsServiceBase
    {
        private readonly ITPSService _tpsService;
        private readonly IGPSService _gpsService;
        private readonly IGTPSService _gasAdjustedTPSService;

        public GeneralService(ITPSService tpsService, IGPSService gpsService, IGTPSService gasAdjustedTPSService, EthtpsContext context, IEnumerable<IHistoricalDataProvider> historicalDataServices, IDataUpdaterStatusService dataUpdaterStatusService) : base(context, historicalDataServices, dataUpdaterStatusService)
        {
            _tpsService = tpsService;
            _gpsService = gpsService;
            _gasAdjustedTPSService = gasAdjustedTPSService;
        }


        public IEnumerable<string> Networks()
        {
            IEnumerable<string> result;
            lock (Context.LockObj)
            {
                result = Context.Networks.Select(x => x.Name);
            }
            return result;
        }


        public IEnumerable<TimeInterval> Intervals() => TimeIntervals();

        public IEnumerable<ProviderResponseModel> Providers(string subchainsOf)
        {
            if (string.IsNullOrWhiteSpace(subchainsOf) || subchainsOf.LossyCompareTo(Constants.ALL))
            {
                return AllProviders;
            }
            return Enumerable.Where(AllProviders, x => x.Name.LossyCompareTo(subchainsOf));
        }

        public IDictionary<string, string> ColorDictionary()
        {
            IDictionary<string, string> result;
            lock (Context.LockObj)
            {
                result = AllProviders.Select(x => new { x.Name, x.Color, x.Enabled }).Where(x => x.Enabled).ToDictionary(x => x.Name, x => x.Color);
            }
            return result;
        }


        public IDictionary<string, string> ProviderTypesColorDictionary()
        {
            IDictionary<string, string> result;
            lock (Context.LockObj)
            {
                result = Context.ProviderTypes.ToDictionary(x => x.Name, x => x.Color);
            }
            return result;
        }

        private static Dictionary<(bool IncludeSidechains, string Network, string Smoothing), (Dictionary<string, object> LastData, DateTime LastGetTime)> _instantDataDictionary = new();

        public async Task<IDictionary<string, object>> InstantDataAsync(ProviderQueryModel model, string smoothing = "")
        {
            TimeInterval interval = TimeInterval.Instant;
            if (!string.IsNullOrWhiteSpace(smoothing))
            {
                interval = Enum.Parse<TimeInterval>(smoothing);
            }
            (bool IncludeSidechains, string Network, string) key = (model.IncludeSidechains, model.Network, interval.ToString());
            if (!_instantDataDictionary.ContainsKey(key))
            {
                _instantDataDictionary.Add(key, (new Dictionary<string, object>(), DateTime.Now.Subtract(TimeSpan.FromSeconds(5))));
            }
            bool returnDefault = DateTime.Now.Subtract(_instantDataDictionary[key].LastGetTime).TotalSeconds < 3;
            if (!returnDefault)
            {
                _instantDataDictionary[key] = (new Dictionary<string, object>(), DateTime.Now);
                (Dictionary<string, object> LastData, DateTime LastGetTime) result = _instantDataDictionary[key];
                switch (interval)
                {
                    case TimeInterval.Instant:
                        result.LastData.Add("tps", await _tpsService.InstantAsync(model));
                        result.LastData.Add("gps", await _gpsService.InstantAsync(model));
                        result.LastData.Add("gasAdjustedTPS", await _gasAdjustedTPSService.InstantAsync(model));
                        break;
                    case TimeInterval.OneWeek:
                        TimeInterval nextInterval = TimeInterval.OneMonth;
                        result.LastData.Add("tps", Enumerable.Where(await _tpsService.GetAsync(model, nextInterval), x => x.Value.Any()).ToDictionary(x => x.Key, x => new List<DataPoint>() { new DataPoint() { Value = x.Value.TakeLast(7).Average(x => (x.Data.FirstOrDefault() == null) ? 0 : x?.Data?.FirstOrDefault()?.Value).GetValueOrDefault() } }));

                        result.LastData.Add("gps", Enumerable.Where(await _gpsService.GetAsync(model, nextInterval), x => x.Value.Any()).ToDictionary(x => x.Key, x => new List<DataPoint>() { new DataPoint() { Value = x.Value.TakeLast(7).Average(x => (x.Data.FirstOrDefault() == null) ? 0 : x?.Data?.FirstOrDefault()?.Value).GetValueOrDefault() } }));

                        result.LastData.Add("gasAdjustedTPS", Enumerable.Where(await _gasAdjustedTPSService.GetAsync(model, nextInterval), x => x.Value.Any()).ToDictionary(x => x.Key, x => new List<DataPoint>() { new DataPoint() { Value = x.Value.TakeLast(7).Average(x => x?.Data?.FirstOrDefault()?.Value).GetValueOrDefault() } }));
                        break;
                    default:
                        nextInterval = GetNextIntervalForInstantData(interval);
                        result.LastData.Add("tps", (await _tpsService.GetAsync(model, nextInterval)).ToDictionary(x => x.Key, x => new List<DataPoint>() { x.Value.Last().Data.First() }));
                        result.LastData.Add("gps", (await _gpsService.GetAsync(model, nextInterval)).ToDictionary(x => x.Key, x => new List<DataPoint>() { x.Value.Last().Data.First() }));
                        result.LastData.Add("gasAdjustedTPS", (await _gasAdjustedTPSService.GetAsync(model, nextInterval)).ToDictionary(x => x.Key, x => new List<DataPoint>() { x.Value.Last().Data.First() }));
                        break;
                }
            }
            return _instantDataDictionary[key].LastData;
        }

        private static TimeInterval GetNextIntervalForInstantData(TimeInterval interval)
        {
            switch (interval)
            {
                case TimeInterval.OneMinute:
                    return TimeInterval.OneHour;
                case TimeInterval.OneHour:
                    return TimeInterval.OneDay;
                case TimeInterval.OneDay:
                    return TimeInterval.OneMonth;
                case TimeInterval.OneMonth:
                    return TimeInterval.OneYear;
                default:
                    return TimeInterval.OneDay;
            }
        }

        public async Task<IDictionary<string, object>> MaxAsync(ProviderQueryModel model)
        {
            Dictionary<string, object> result = new();
            IDictionary<string, DataPoint> maxGPS = await _gpsService.MaxAsync(model);
            result.Add("tps", await _tpsService.MaxAsync(model));
            result.Add("gps", maxGPS);
            result.Add("gasAdjustedTPS", await _gasAdjustedTPSService.MaxAsync(model));
            return result;
        }

        /// <summary>
        /// Used for displaying chart buttons.
        /// </summary>

        public async Task<IEnumerable<TimeInterval>> GetIntervalsWithDataAsync(ProviderQueryModel model)
        {
            List<TimeInterval> result = new();
            foreach (var interval in TimeIntervals())
            {
                try
                {
                    int count = (await _tpsService.GetAsync(model, interval))[model.Provider].Count();
                    if (count > 1)
                    {
                        if (interval == TimeInterval.All && count < 12)
                            continue;

                        result.Add(interval);
                    }
                }
                catch { }
            }
            return result;
        }


        public async Task<IEnumerable<string?>> GetUniqueDataYearsAsync(ProviderQueryModel model)
        {
            IEnumerable<string?>? entries = (await _tpsService.GetAsync(model, TimeInterval.All))[model.Provider]?.Select(x => x.Data.FirstOrDefault()?.Date.Year.ToString())?.OrderBy(x => x)?.Distinct();
            var preResult = (entries?.SafeAny(x => x != null)).GetValueOrDefault();
            var e = Enumerable.Empty<string?>();
            return preResult ? (entries ?? e).ToList() : e;
        }

        public async Task<AllDataModel> GetAllDataAsync(string network)
        {
            ProviderQueryModel allDataModel = new()
            {
                Provider = Constants.ALL,
                Network = network
            };
            return new AllDataModel()
            {
                Providers = AllProviders.Select(x => new ProviderModel()
                {
                    Name = x.Name,
                    Type = x.Type ?? string.Empty
                }).ToArray(),
                AllTPSData = Intervals().Select(async interval =>
                new
                {
                    interval,
                    data = (await _tpsService.GetAsync(allDataModel, interval))
                })
                .ToDictionary(x => x.Result.interval, x => x.Result.data),
                MaxData = await MaxAsync(allDataModel),
                AllGPSData = Intervals().Select(async interval => new { interval, data = (await _gpsService.GetAsync(allDataModel, interval)) }).ToDictionary(x => x.Result.interval, x => x.Result.data),
                AllGasAdjustedTPSData = Intervals().Select(async interval => new
                {
                    interval,
                    data = await _gasAdjustedTPSService.GetAsync(allDataModel, interval)
                })
                .ToDictionary(x => x.Result.interval, x => x.Result.data),
            };
        }

        /*
        
        public async Task<HomePageViewModel> HomePageModelAsync(string network = "Mainnet")
        {
            return new HomePageViewModel()
            {
                InstantTPS = await InstantTPSAsync(),
                ColorDictionary = _Providers.ToDictionary(x => x.Name, x => x.Color),
                ProviderData = _Providers.Select(x => new ProviderInfo()
                {
                    Name = x.Name,
                    MaxTPS = MaxTPS(x.Name).FirstOrDefaultOrDefault().Data.FirstOrDefaultOrDefault().TPS,
                    Type = x.Type
                }),
                //TPSData = await BuildTPSDataAsync(network)
            };
        }
        */
    }
}
