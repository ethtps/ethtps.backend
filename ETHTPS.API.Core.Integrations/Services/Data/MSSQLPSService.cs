using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.HistoricalDataProviders;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.Data
{
    public abstract class MSSQLPSService : MSSSQLDataServiceBase
    {
        private readonly Func<TimedTPSAndGasData, DataPoint> _transformFunction;
        private readonly Func<TimedTPSAndGasData, DataPoint> _reducedTransformFunction;

        public MSSQLPSService(EthtpsContext context, IEnumerable<IHistoricalDataProvider> historicalDataServices, IDataUpdaterStatusService dataUpdaterStatusService, Func<TimedTPSAndGasData, DataPoint> transformFunction, Func<TimedTPSAndGasData, DataPoint> reducedTransformFunction) : base(context, historicalDataServices, dataUpdaterStatusService)
        {
            _transformFunction = transformFunction;
            _reducedTransformFunction = reducedTransformFunction;
        }

        public Task<IDictionary<string, DataPoint>> MaxAsync(ProviderQueryModel model) => Task.FromResult(Max(model, DataType.TPS));

        public Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetAsync(ProviderQueryModel model, TimeInterval interval)
        {
            Dictionary<string, IEnumerable<DataResponseModel>> result = new();
            lock (Context.LockObj)
            {
                if (model.Provider.ToUpper() == "ALL")
                {
                    foreach (var p in AllProviders.Where(x => x.Enabled).ToList())
                    {
                        if (!model.IncludeSidechains)
                        {
                            if (IsSidechain(p.Name))
                            {
                                continue;
                            }
                        }
                        result[p.Name] = GetHistoricalData(ProviderQueryModel.FromProviderName(p.Name), interval).Select(x => new DataResponseModel()
                        {
                            Data = new List<DataPoint>()
                        {
                            { _transformFunction(x) }
                        }
                        });
                    }
                }
                else
                {
                    result[model.Provider] = GetHistoricalData(model, interval).Select(x => new DataResponseModel()
                    {
                        Data = new List<DataPoint>()
                    {
                        { _transformFunction(x) }
                    }
                    });
                }
            }
            return Task.FromResult((IDictionary<string, IEnumerable<DataResponseModel>>)result);
        }

        public async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetMonthlyDataByYearAsync(ProviderQueryModel model, int year)
        {
            IDictionary<string, IEnumerable<DataResponseModel>> data = await GetAsync(model, TimeInterval.All);
            foreach (string key in data.Keys)
            {
                data[key] = data[key].Where(x => x.Data.First().Date.Year == year);
            }
            return data;
        }

        public Task<IDictionary<string, IEnumerable<DataPoint>>> InstantAsync(ProviderQueryModel model)
        {
            List<DataResponseModel> result = new();
            lock (Context.LockObj)
            {
                foreach (var p in AllProviders.Where(x => x.Enabled).ToList())
                {
                    if (!model.IncludeSidechains)
                    {
                        if (p.Type == "Sidechain")
                        {
                            continue;
                        }
                    }
                    if (Context.TpsandGasDataLatests.Any(x => x.Provider == p.Id))
                    {
                        TpsandGasDataLatest entry = Context.TpsandGasDataLatests.First(x => x.Provider == p.Id);
                        result.Add(new DataResponseModel()
                        {
                            Provider = p.Name,
                            Data = new List<DataPoint>()
                        {
                            { _reducedTransformFunction(entry) }
                        }
                        });
                    }
                }
            }
            return Task.FromResult((IDictionary<string, IEnumerable<DataPoint>>)result.ToDictionary(x => x.Provider, x => x.Data.AsEnumerable()));
        }

    }
}
