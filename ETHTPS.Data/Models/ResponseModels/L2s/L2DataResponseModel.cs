﻿using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace ETHTPS.Data.Core.Models.ResponseModels.L2s
{
    /// <summary>
    /// An object used for responsding to L2 data requests
    /// </summary>
    public sealed class L2DataResponseModel : IAnalysisParameters
    {

        public L2DataResponseModel() { }
        public L2DataResponseModel(IAnalysisParameters analysisParameters)
        {
            if (analysisParameters != null)
            {
                IncludeSimpleAnalysis = analysisParameters.IncludeSimpleAnalysis;
                IncludeComplexAnalysis = analysisParameters.IncludeComplexAnalysis;
            }
        }
        /// <summary>
        /// This field is set when data is requested for a single provider
        /// </summary>
        public Dataset? Data { get; set; }

        private IEnumerable<Dataset>? _datasets;
        /// <summary>
        /// This field is set when data is requested for multiple providers.
        /// </summary>
        public IEnumerable<Dataset>? Datasets
        {
            get
            {
                return _datasets;
            }
            set
            {
                if (value == null) return;
                //Use the Data property if there's only one dataset
                if (value.Count() == 1)
                {
                    Data = value.FirstOrDefault();
                }
                else
                {
                    _datasets = value;
                }
            }
        }
        public SimpleMultiDatasetAnalysis? SimpleAnalysis
        {
            get
            {
                if (IncludeSimpleAnalysis && Datasets != null && Datasets.Any(x => x.DataPoints.Count() > 0))
                    return new SimpleMultiDatasetAnalysis(Datasets);
                return null;
            }
        }
        public DataType? DataType { get; set; } = null;
        [JsonIgnore]
        public bool IncludeSimpleAnalysis { get; set; } = false;
        [JsonIgnore]
        public bool IncludeComplexAnalysis { get; set; } = false;
    }
}
