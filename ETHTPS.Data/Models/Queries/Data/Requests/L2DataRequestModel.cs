﻿using System;
using System.Collections.Generic;
using System.Linq;

using ETHTPS.Core;
using ETHTPS.Data.Core.Extensions.DateTimeExtensions;
using ETHTPS.Data.Core.Models.DataPoints.XYPoints;
using ETHTPS.Data.Core.Models.ResponseModels.L2s;

using Newtonsoft.Json;

namespace ETHTPS.Data.Core.Models.Queries.Data.Requests
{
    public sealed class L2DataRequestModel : ProviderQueryModel, IAnalysisParameters, ICachedKey, IGuidEntity
    {
        public L2DataRequestModel()
        {
            base.Provider = null;
        }

        [JsonIgnore]
        public TimeInterval AutoInterval
        {
            get
            {
                StartDate ??= DateTime.Now;
                EndDate ??= DateTime.Now;
                return (EndDate.Value - StartDate.Value).GetClosestInterval();
            }
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public BucketOptions BucketOptions { get; set; } = new();
        public XPointType ReturnXAxisType { get; set; } = XPointType.Date;
        public bool IncludeEmptyDatasets { get; set; } = false;

        /// <summary>
        /// Very useful when trying to display a lot of data.
        /// </summary>
        public DatasetMergeOptions MergeOptions { get; set; } = new();

        /// <summary>
        /// Whether to include basic data analysis such as min, max, average in the result
        /// </summary>
        public bool IncludeSimpleAnalysis { get; set; } = true;

        /// <summary>
        /// Whether to include more complex analysis in the result
        /// </summary>
        public bool IncludeComplexAnalysis { get; set; } = false;

        /// <summary>
        /// Used in case data for multiple providers is requested. 
        /// </summary>
        public List<string>? Providers { get; set; }

        [JsonIgnore]
        public IEnumerable<string> AllDistinctProviders => (Providers ?? Enumerable.Empty<string>()).Concat(new string[] { Provider }).Distinct().Where(x => !string.IsNullOrWhiteSpace(x));

        public string Guid { get; set; } = System.Guid.NewGuid().ToString();

        public ValidationResult Validate(IEnumerable<string> availableProviders)
        {
            if (StartDate == null && EndDate == null)
            {
                return ValidationResult.InvalidFor($"Both {nameof(StartDate)} and {nameof(EndDate)} are null");
            }
            if (StartDate != null && EndDate != null)
            {
                if (EndDate < StartDate)
                {
                    return ValidationResult.InvalidFor($"{nameof(EndDate)} can't be earlier than {nameof(StartDate)}");
                }
            }
            if (BucketOptions.CustomBucketSize.HasValue)
            {
                if (BucketOptions.BucketSize != TimeInterval.Auto)
                {
                    return ValidationResult.InvalidFor($"Can't specify both {nameof(BucketOptions.BucketSize)} and {nameof(BucketOptions.CustomBucketSize)} at the same time");
                }
            }
            if (AllDistinctProviders.Count() == 0)
            {
                return ValidationResult.InvalidFor("No provider(s) specified");
            }
            foreach (var provider in AllDistinctProviders)
            {
                if (provider == Constants.All)
                    break;
                if (!availableProviders.Contains(provider))
                {
                    return ValidationResult.InvalidFor($"Provider \"{provider}\" is not supported. Spelling is case-sensitive.");
                }
            }
            if (BucketOptions.CustomBucketSize.HasValue)
            {
                if (BucketOptions.BucketSize != TimeInterval.Auto)
                {
                    return ValidationResult.InvalidFor($"Can't specify both {nameof(BucketOptions.BucketSize)} and {nameof(BucketOptions.CustomBucketSize)}");
                }

                if (BucketOptions.CustomBucketSize.Value < TimeSpan.FromMinutes(1))
                {
                    return ValidationResult.InvalidFor($"Custom bucket size is too small. Minimum allowed value is 1 minute.");
                }
            }
            if (!AllDistinctProviders.Any())
            {
                return ValidationResult.InvalidFor("No provider(s) specified");
            }
            return ValidationResult.Valid;
        }
        public static string GenerateCacheKeyFromGuid(string guid) => $"L2DataRequest:{guid}";

        public new string ToCacheKey() => GenerateCacheKeyFromGuid(Guid);
    }
}
