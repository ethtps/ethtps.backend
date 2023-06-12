using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GTPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.TPS;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.DataPoints.XYPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Core.Models.ResponseModels.L2s;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.Extensions.Logging;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataServices
{
    public sealed class AggregatedDataservice : IAggregatedDataService
    {
        private readonly ITPSService _tpsService;
        private readonly IGPSService _gpsService;
        private readonly IGTPSService _gtpsService;
        private readonly ILogger<AggregatedDataservice>? _logger;
        private readonly string[] _allAvailableProviders;

        public AggregatedDataservice(ITPSService tpsService, IGPSService gpsService, IGTPSService gtpsService, EthtpsContext context, ILogger<AggregatedDataservice>? logger)
        {
            _tpsService = tpsService;
            _gpsService = gpsService;
            _gtpsService = gtpsService;
            _logger = logger;
            _allAvailableProviders = context.Providers.Select(x => x.Name).ToArray();
        }

        private async Task<List<DataResponseModel>> GetDataAsync2(L2DataRequestModel requestModel, DataType dataType, TimeInterval? interval)
        {
            requestModel.Validate(_allAvailableProviders).ThrowIfInvalid();
            if (requestModel.StartDate != null && requestModel.EndDate != null)
            {
                return dataType switch
                {
                    DataType.TPS => (await _tpsService.GetAsync(requestModel)).ToList(),
                    DataType.GPS => (await _gpsService.GetAsync(requestModel)).ToList(),
                    DataType.GasAdjustedTPS => (await _gtpsService.GetAsync(requestModel)).ToList(),
                    _ => throw new ArgumentException($"{dataType} is not supported."),
                };
            }
            else if (interval != null)
            {
                return dataType switch
                {
                    DataType.TPS => await GetTPSAsync(requestModel, interval.Value),
                    DataType.GPS => await GetGPSAsync(requestModel, interval.Value),
                    DataType.GasAdjustedTPS => await GetGTPSAsync(requestModel, interval.Value),
                    _ => throw new ArgumentException($"{dataType} is not supported."),
                };
            }
            throw new ArgumentException($"No start date, end date or time interval specified");
        }

        public async Task<List<DataResponseModel>> GetDataAsync(L2DataRequestModel requestModel, DataType dataType, TimeInterval interval) => await GetDataAsync2(requestModel, dataType, interval);

        public async Task<List<DataResponseModel>> GetDataAsync(L2DataRequestModel requestModel, DataType dataType) => await GetDataAsync2(requestModel, dataType, null);

        public Task<L2DataResponseModel> GetDataAsync(L2DataRequestModel requestModel, DataType dataType, IPSDataFormatter formatter)
        {
            var result = new L2DataResponseModel(requestModel)
            {
                DataType = dataType,
                Datasets = requestModel.AllDistinctProviders
                ?.Select(async providerName =>
                {
                    requestModel.Provider = providerName;
                    return new Dataset(formatter.Format(await GetDataAsync(requestModel, dataType, requestModel.AutoInterval), requestModel), providerName, requestModel.IncludeSimpleAnalysis, requestModel.IncludeComplexAnalysis);
                })
                ?.Select(task => task.Result)
                .Where(x => !requestModel.IncludeEmptyDatasets ? x.DataPoints.Count() > 0 : true)
                .OrderByDescending(x => x.DataPoints.Average(x => x.Y))
                .ToArray()
            };
            if (result.Datasets != null)
                result.Datasets = formatter.MakeEqualLength(result.Datasets, requestModel.ReturnXAxisType);
            if (requestModel.MergeOptions.MergePercentage.HasValue)
            {
                SimpleMultiDatasetAnalysis analysis = result.SimpleAnalysis ?? new(result.Datasets);
                if (result.Datasets != null && result.Datasets.Any())
                {
                    var toBeMergedCount = Enumerable.Range(1, result.Datasets.Count() - 1).Where(i => (new SimpleMultiDatasetAnalysis(result.Datasets.TakeLast(i)).Mean * 100 / analysis.Mean < requestModel.MergeOptions.MergePercentage.Value)).Count();

                    var toBeMerged = result.Datasets.TakeLast(toBeMergedCount);

                    result.Datasets = result.Datasets.Take(result.Datasets.Count() - toBeMergedCount);

                    List<DatedXYDataPoint> mergedSet = new();
                    for (int i = 0; i < toBeMerged.First().DataPoints.Count(); i++)
                    {
                        var points = toBeMerged.Select(x => x.DataPoints.ElementAt(i));
                        mergedSet.Add(new DatedXYDataPoint()
                        {
                            X = points.First().ToDatedXYDataPoint().X,
                            Y = Math.Round(points.Average(x => x.Y), 2)
                        });
                    }
                    result.Datasets = result.Datasets.Concat(new[]
                    {
                        new Dataset(mergedSet.Convert
                        (requestModel.ReturnXAxisType), "Others", requestModel.IncludeSimpleAnalysis, requestModel.IncludeComplexAnalysis)
                    }).ToArray();
                }
            }
            if (requestModel.MergeOptions.MaxCount.HasValue)
            {
                result.Datasets = result.Datasets?.Take(requestModel.MergeOptions.MaxCount.Value);
            }
            return Task.FromResult(result);
        }

        public async Task<List<DataResponseModel>> GetGPSAsync(ProviderQueryModel requestModel, TimeInterval interval)
        {
            return await _gpsService.GetGPSAsync(requestModel, interval);
        }

        public async Task<List<DataResponseModel>> GetGTPSAsync(ProviderQueryModel requestModel, TimeInterval interval)
        {
            return await _gtpsService.GetGTPSAsync(requestModel, interval);
        }

        public async Task<List<DataResponseModel>> GetTPSAsync(ProviderQueryModel requestModel, TimeInterval interval)
        {
            return await _tpsService.GetTPSAsync(requestModel, interval);
        }
    }
}
