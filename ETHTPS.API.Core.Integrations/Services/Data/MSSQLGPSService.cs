using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.HistoricalDataProviders;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.Data
{
    public sealed class MSSQLGPSService : MSSQLPSService, IGPSService
    {
        public MSSQLGPSService(EthtpsContext context, IEnumerable<IHistoricalDataProvider> historicalDataServices, IDataUpdaterStatusService dataUpdaterStatusService) : base(context, historicalDataServices, dataUpdaterStatusService, x => new DataPoint() { Value = x.AverageGps, Date = x.StartDate }, x => new DataPoint() { Value = x.AverageGps })
        {
        }

        public async Task<List<DataResponseModel>> GetGPSAsync(ProviderQueryModel requestModel, TimeInterval interval) => (await GetAsync(requestModel, interval)).SelectMany((x) => x.Value).ToList();
    }
}

