using ETHTPS.API.BIL.Infrastructure.Services.DataServices.TPS;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataEntries;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;

namespace ETHTPS.Data.Integrations.InfluxIntegration.ProviderServices.DataProviders
{
    public sealed class InfluxTPSService : ITPSService
    {
        private readonly IInfluxWrapper _influxWrapper;

        public InfluxTPSService(IInfluxWrapper influxWrapper)
        {
            _influxWrapper = influxWrapper;
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
                    Value = entry.TransactionCount
                };

                if (list.Count > 0)
                {
                    var lastDataPoint = list.Last().Data.Last();
                    var timeDiffInSeconds = (dataPoint.Date - lastDataPoint.Date).TotalSeconds;
                    if (timeDiffInSeconds > 0)
                    {
                        dataPoint.Value = (dataPoint.Value - lastDataPoint.Value) / timeDiffInSeconds;
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

        public Task<List<DataResponseModel>> GetTPSAsync(ProviderQueryModel requestModel, TimeInterval interval)
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
