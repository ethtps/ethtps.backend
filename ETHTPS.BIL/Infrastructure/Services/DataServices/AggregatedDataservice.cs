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
#pragma warning disable CA1860

namespace ETHTPS.API.BIL.Infrastructure.Services.DataServices
{
    public sealed class AggregatedDataservice : IAggregatedDataService
    {
        private readonly ITPSService _tpsService;
        private readonly IGPSService _gpsService;
        private readonly IGTPSService _gtpsService;
        private readonly ILogger<AggregatedDataservice>? _logger;
        private readonly string[] _allAvailableProviders;
        private readonly DeedleTimeSeriesFormatter _dataFormatter = new();

        public AggregatedDataservice(ITPSService tpsService, IGPSService gpsService, IGTPSService gtpsService, EthtpsContext context, ILogger<AggregatedDataservice>? logger)
        {
            _tpsService = tpsService;
            _gpsService = gpsService;
            _gtpsService = gtpsService;
            _logger = logger;
            _allAvailableProviders = context.Providers.Select(x => x.Name).ToArray();
        }

        private async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetDataAsync2(L2DataRequestModel requestModel, DataType dataType, TimeInterval? interval)
        {
            requestModel.Validate(_allAvailableProviders).ThrowIfInvalid();
            if (requestModel is { StartDate: not null, EndDate: not null })
            {
                return dataType switch
                {
                    DataType.TPS => (await _tpsService.GetAsync(requestModel)),
                    DataType.GPS => (await _gpsService.GetAsync(requestModel)),
                    DataType.GasAdjustedTPS => (await _gtpsService.GetAsync(requestModel)),
                    _ => throw new ArgumentException($"{dataType} is not supported."),
                };
            }
            else if (interval != null)
            {
                IDictionary<string, IEnumerable<DataResponseModel>> format(List<DataResponseModel> data)
                {
                    var groups = data.GroupBy(x => x.Provider);
                    return groups.ToDictionary(x => x.Key, x => x.AsEnumerable());
                }
                return dataType switch
                {
                    DataType.TPS => format(await GetTPSAsync(requestModel, interval.Value)),
                    DataType.GPS => format(await GetGPSAsync(requestModel, interval.Value)),
                    DataType.GasAdjustedTPS => format(await GetGTPSAsync(requestModel, interval.Value)),
                    _ => throw new ArgumentException($"{dataType} is not supported."),
                };
            }
            throw new ArgumentException($"No start date, end date or time interval specified");
        }

        public async Task<IDictionary<string, IEnumerable<DataResponseModel>>> GetDataAsync(L2DataRequestModel requestModel, DataType dataType, TimeInterval interval) => await GetDataAsync2(requestModel, dataType, interval);

        public Task<L2DataResponseModel> GetDataAsync(L2DataRequestModel requestModel, DataType dataType)
        {
            requestModel = requestModel ?? throw new ArgumentNullException(nameof(requestModel));

            var datasets = GetDatasets(requestModel, dataType) ?? Enumerable.Empty<Dataset>();

            var formattedDatasets = _dataFormatter.MakeEqualLength(datasets, requestModel.ReturnXAxisType);

            if (requestModel.MergeOptions.MergePercentage.HasValue)
            {
                formattedDatasets = HandlePercentageMerge(requestModel, formattedDatasets);
            }

            if (requestModel.MergeOptions.MaxCount.HasValue)
            {
                formattedDatasets = HandleMaxCountMerge(requestModel, formattedDatasets);
            }

            var result = new L2DataResponseModel(requestModel)
            {
                DataType = dataType,
                Datasets = formattedDatasets ?? Array.Empty<Dataset>()
            };

            return Task.FromResult(result);
        }

        private IEnumerable<Dataset>? GetDatasets(L2DataRequestModel requestModel, DataType dataType)
        {
            return requestModel.AllDistinctProviders
                ?.Select(async providerName =>
                {
                    requestModel.Provider = providerName;
                    var data = await GetDataAsync(requestModel, dataType, requestModel.AutoInterval);
                    return new Dataset(_dataFormatter.Format(data.ContainsKey(providerName) ? data[providerName] : Enumerable.Empty<DataResponseModel>(), requestModel), providerName, requestModel.IncludeSimpleAnalysis, requestModel.IncludeComplexAnalysis);
                })
                ?.Select(task => task.Result)
                .Where(x => !requestModel.IncludeEmptyDatasets || x.DataPoints.Any())
                .OrderByDescending(x =>
                {
                    if (!x.DataPoints.Any()) return 0;
                    return x.DataPoints.Average(p => p.Y);
                })
                .ToArray();
        }

        private Dataset[] HandlePercentageMerge(L2DataRequestModel requestModel, IEnumerable<Dataset> datasets)
        {
            var enumerable = datasets as Dataset[] ?? datasets.ToArray();
            SimpleMultiDatasetAnalysis analysis = new(enumerable);
            if (!enumerable.Any()) return enumerable;
            var toBeMergedCount = Enumerable.Range(1, enumerable.Length - 1).Count(i => requestModel.MergeOptions.MergePercentage != null && (new SimpleMultiDatasetAnalysis(enumerable.TakeLast(i)).Mean * 100 / analysis.Mean < requestModel.MergeOptions.MergePercentage.Value));
            var toBeMerged = enumerable.TakeLast(toBeMergedCount).ToList();

            datasets = enumerable.Take(enumerable.Length - toBeMergedCount).ToArray();

            var mergedSet = MergeSets(toBeMerged);

            datasets = datasets.Concat(new[]
            {
                new Dataset(mergedSet.Convert(requestModel.ReturnXAxisType), "Others", requestModel.IncludeSimpleAnalysis, requestModel.IncludeComplexAnalysis)
            }).ToArray();
            return enumerable;
        }

        private static List<DatedXYDataPoint> MergeSets(IReadOnlyCollection<Dataset> toBeMerged)
        {
            List<DatedXYDataPoint> mergedSet = new();
            for (var i = 0; i < toBeMerged.First().DataPoints.Count(); i++)
            {
                var points = toBeMerged.Select(x => x.DataPoints.ElementAt(i)).ToList();
                mergedSet.Add(new DatedXYDataPoint()
                {
                    X = points.First().ToDatedXYDataPoint().X,
                    Y = Math.Round(points.Average(x => x.Y), 2)
                });
            }

            return mergedSet.ToList();
        }

        private static IEnumerable<Dataset> HandleMaxCountMerge(L2DataRequestModel requestModel, IEnumerable<Dataset> datasets)
        {
            var result = datasets as Dataset[] ?? datasets.ToArray();
            return result.ToList().Take(requestModel.MergeOptions.MaxCount ?? result.Length).ToArray();
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
