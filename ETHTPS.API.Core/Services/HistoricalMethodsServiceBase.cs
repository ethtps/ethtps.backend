using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.HistoricalDataProviders;

namespace ETHTPS.API.Core.Services
{
    public abstract class HistoricalMethodsServiceBase : ContextServiceBase
    {
        protected IEnumerable<IHistoricalDataProvider> HistoricalDataServices { get; set; }
        protected HistoricalMethodsServiceBase(EthtpsContext context, IEnumerable<IHistoricalDataProvider> historicalDataServices, IDataUpdaterStatusService dataUpdaterStatus) : base(context, dataUpdaterStatus)
        {
            HistoricalDataServices = historicalDataServices;
        }

        protected IEnumerable<TimedTPSAndGasData> GetHistoricalData(ProviderQueryModel model, TimeInterval interval)
        {
            if (HistoricalDataServices.Any(x => x.Interval == interval))
            {
                var dataProvider = HistoricalDataServices.First(x => x.Interval == interval);
                return dataProvider.GetData(model);
            }
            else
            {
                return new List<TimedTPSAndGasData>();
            }
        }
    }
}
