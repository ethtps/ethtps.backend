using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.HistoricalDataServices;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.Data
{
    public class MSSQLGPSService : MSSSQLDataServiceBase, IGPSService
    {
        public MSSQLGPSService(EthtpsContext context, IEnumerable<IHistoricalDataProvider> historicalDataServices, IDataUpdaterStatusService dataUpdaterStatusService) : base(context, historicalDataServices, dataUpdaterStatusService)
        {
        }

        public IDictionary<string, DataPoint> Max(ProviderQueryModel model) => Max(model, DataType.GPS);

        public IDictionary<string, IEnumerable<DataPoint>> Instant(ProviderQueryModel model)
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
                            Data = new List<DataPoint>() {{ new DataPoint() { Value = entry.Gps} }
                    }
                        });
                    }
                }
            }
            return result.ToDictionary(x => x.Provider, x => x.Data.AsEnumerable());
        }


        public IDictionary<string, IEnumerable<DataResponseModel>> GetMonthlyDataByYear(ProviderQueryModel model, int year)
        {
            IDictionary<string, IEnumerable<DataResponseModel>> data = Get(model, TimeInterval.All);
            foreach (string key in data.Keys)
            {
                data[key] = data[key].Where(x => x.Data.First().Date.Year == year);
            }
            return data;
        }

        public IDictionary<string, IEnumerable<DataResponseModel>> Get(ProviderQueryModel model, TimeInterval interval)
        {
            Dictionary<string, IEnumerable<DataResponseModel>> result = new();
            lock (Context.LockObj)
            {
                if (model.Provider.ToUpper() == Constants.ALL.ToUpper())
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
                            { new DataPoint(){ Value = x.AverageGps, Date = x.StartDate} }
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
                            { new DataPoint(){Value = x.AverageGps, Date = x.StartDate} }
                        }
                    });
                }
            }
            return result;
        }

        public List<DataResponseModel> GetGPS(ProviderQueryModel requestModel, TimeInterval interval) => Get(requestModel, interval).SelectMany((x) => x.Value).ToList();
    }
}
