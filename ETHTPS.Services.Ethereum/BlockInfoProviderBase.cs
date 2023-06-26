using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Validation.Exceptions;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries;

namespace ETHTPS.Services.Ethereum
{
    /// <summary>
    /// Provides a base class for building block info providers based on an <see cref="DBConfigurationProviderWithCache"/>
    /// </summary>
    public abstract class BlockInfoProviderBase : IHTTPBlockInfoProvider
    {
        protected IProviderConfigurationStringProvider _configurationProvider;
        protected IEnumerable<IConfigurationString> _configurationStrings;
        protected string _providerName;
        protected readonly HttpClient _httpClient;
        #region Common properties and selectors

        private string _endpoint;
        protected string Endpoint
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_endpoint))
                {
                    _endpoint = GetEndpoint();
                }
                return _endpoint;
            }
            set => _endpoint = value;
        }

        protected string TXCountSelector => PartialMatchOrThrow("TXCount");
        protected string GasUsedSelector => PartialMatchOrThrow("GasUsed", "Gas");
        protected string DateSelector => PartialMatchOrThrow("DateSelector", "Date");
        protected string BlockHeightSelector => PartialMatchOrThrow("BlockHeight");
        protected string APIKey => PartialMatchOrThrow("APIKey");
        protected string Secret => PartialMatchOrThrow("Secret");
        protected string ProjectID => PartialMatchOrThrow("ProjectID");
        private string? GetEndpoint()
        {

            var valid = new string[] { "Endpoint", "EndpointBase", "BaseURL", "URL" };
            foreach (var configString in _configurationStrings)
            {
                if (valid.Select(x => x.ToUpper()).Any(x => configString.Name.ToUpper().EndsWith(x)))
                {
                    return configString.Value;
                }
            }
            return null;
        }
        protected string PartialMatchOrThrow(params string[] partialNames)
        {
            foreach (var configString in _configurationStrings)
            {
                if (partialNames.Select(x => x.ToUpper()).Any(partialName => configString.Name.ToUpper().Contains(partialName.ToUpper())))
                {
                    return configString.Value;
                }
            }
            throw new ConfigurationStringNotFoundException(string.Join(", ", string.Join(", ", partialNames)), _providerName);
        }

        #endregion

        protected BlockInfoProviderBase(DBConfigurationProviderWithCache configurationProvider, string providerName)
        {
            _configurationStrings = configurationProvider.GetConfigurationStringsForProvider(providerName);
            _providerName = providerName;
            _httpClient = new HttpClient();
            if (Endpoint != null) _httpClient.BaseAddress = new Uri(Endpoint);
        }

        private double _blockTimeSeconds = double.NaN;
        public double? BlockTimeSeconds
        {
            get
            {
                if (double.IsNaN(_blockTimeSeconds))
                {
                    try
                    {
                        _blockTimeSeconds = double.Parse(PartialMatchOrThrow("BlockTime"));
                    }
                    catch
                    {
                        return null;
                    }
                }
                return _blockTimeSeconds;
            }
            set
            {
                if (value != null)
                    _blockTimeSeconds = (double)value;
            }
        }

        public abstract Task<Block> GetBlockInfoAsync(int blockNumber);
        public abstract Task<Block> GetBlockInfoAsync(DateTime time);
        public abstract Task<Block> GetLatestBlockInfoAsync();
    }
}
