﻿using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.GTPS;
using ETHTPS.API.BIL.Infrastructure.Services.DataServices.TPS;
using ETHTPS.Data.Core;
using ETHTPS.Data.Core.Models.DataPoints;
using ETHTPS.Data.Core.Models.DataPoints.XYPoints;
using ETHTPS.Data.Core.Models.Queries.Data.Requests;
using ETHTPS.Data.Core.Models.ResponseModels.L2s;

using Microsoft.Extensions.Logging;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataServices
{
    public class DependencyInjectionAggregatedDataservice : IAggregatedDataService
    {
        private readonly ITPSService _tpsService;
        private readonly IGPSService _gpsService;
        private readonly IGTPSService _gtpsService;
        private readonly ILogger<DependencyInjectionAggregatedDataservice>? _logger;

        public DependencyInjectionAggregatedDataservice(ITPSService tpsService, IGPSService gpsService, IGTPSService gtpsService, ILogger<DependencyInjectionAggregatedDataservice>? logger)
        {
            _tpsService = tpsService;
            _gpsService = gpsService;
            _gtpsService = gtpsService;
            _logger = logger;
        }

        public async Task<List<DataResponseModel>> GetDataAsync(L2DataRequestModel requestModel, DataType dataType, TimeInterval interval)
        {
            return dataType switch
            {
                DataType.TPS => await GetTPSAsync(requestModel, interval),
                DataType.GPS => await GetGPSAsync(requestModel, interval),
                DataType.GasAdjustedTPS => await GetGTPSAsync(requestModel, interval),
                _ => throw new ArgumentException($"{dataType} is not supported."),
            };
        }

        public async Task<L2DataResponseModel> GetDataAsync(L2DataRequestModel requestModel, DataType dataType, IPSDataFormatter formatter)
        {
            var result = new L2DataResponseModel(requestModel)
            {
                DataType = dataType,
                Datasets = requestModel.Providers?.ToArray().Select(async providerName =>
                {
                    requestModel.Provider = providerName;
                    return new Dataset(formatter.Format(await GetDataAsync(requestModel, dataType, requestModel.AutoInterval), requestModel), providerName, requestModel.IncludeSimpleAnalysis, requestModel.IncludeComplexAnalysis);
                })
                .Select(task => task.Result)
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
            return result;
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
