using ETHTPS.API.Core.Services;
using ETHTPS.Data.Integrations.MSSQL.HistoricalDataServices;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Core.Models.Providers;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ServiceStack;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.Data
{
    public abstract class MSSSQLDataServiceBase : HistoricalMethodsServiceBase
    {
        public MSSSQLDataServiceBase(EthtpsContext context, IEnumerable<IHistoricalDataProvider> historicalDataServices, IDataUpdaterStatusService dataUpdaterStatus) : base(context, historicalDataServices, dataUpdaterStatus)
        {

        }


        protected IEnumerable<ProviderSummaryBase> SafeGetProviderSummaries(ProviderQueryModel model)
        {
            var formatted = Providers().Select(x =>
            new
            {
                x.Name,
                x.Id
            });
            if (model.Provider.EqualsIgnoreCase(Constants.All))
            {
                return formatted.Select(x => new Provider()
                {
                    Name = x.Name,
                    Id = x.Id
                });
            }
            var target = formatted.FirstOrDefault(x => x.Name.EqualsIgnoreCase(model.Provider));
            return target == null? Enumerable.Empty<ProviderSummaryBase>(): new ProviderSummaryBase[]
            {
                 new Provider()
                {
                    Name = target.Name,
                    Id = target.Id
                }
            };
        }

        protected DataPoint SafeGetMax(DataType dataType, ProviderSummaryBase providerSummary, ProviderQueryModel model)
        {
            var entry = Context.TpsandGasDataMaxes.Select(x => 
            new 
            { 
                x.Provider, 
                x.Id,
                 x.Network,
                x.MaxGps,
                x.MaxTps,
                x.MaxGpsblockNumber,
                x.MaxTpsblockNumber,
                x.NetworkNavigation,
                x.Date
            })
                .FirstOrDefault(x => x.Provider == providerSummary.Id && x.NetworkNavigation.Name == model.Network);
            if (entry == null)
            {
                return new DataPoint()
                {
                    Value = 0
                };
            }
            return dataType switch
            {
                DataType.TPS => new DataPoint()
                {
                    Date = entry.Date,
                    Value = entry.MaxTps,
                    BlockNumber = entry.MaxTpsblockNumber
                },
                DataType.GPS => new DataPoint()
                {
                    Date = entry.Date,
                    Value = entry.MaxGps,
                    BlockNumber = entry.MaxGpsblockNumber
                },
                _ => throw new ArgumentOutOfRangeException(dataType.ToString()),
            };
        }
        protected IDictionary<string, DataPoint> Max(ProviderQueryModel model, DataType dataType)
        {
            List<DataResponseModel> result = new();
            lock (Context.LockObj)
            {
                foreach (var p in SafeGetProviderSummaries(model))
                {
                    result.Add(new DataResponseModel()
                    {
                        Provider = p.Name,
                        Data = new List<DataPoint>()
                        {
                            SafeGetMax(dataType, p, model)
                        }
                    });
                }
            }
            return result.ToDictionary(x => x.Provider, x => x.Data.First());
        }
    }
}
