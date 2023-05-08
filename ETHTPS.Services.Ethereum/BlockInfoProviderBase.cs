﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ETHTPS.Configuration;
using ETHTPS.Configuration.Validation.Exceptions;
using ETHTPS.Data.Core.BlockInfo;
using ETHTPS.Data.Core.Models.DataEntries;

namespace ETHTPS.Services.Ethereum
{
    /// <summary>
    /// Provides a base class for building block info providers based on an <see cref="IDBConfigurationProvider"/>
    /// </summary>
    public abstract class BlockInfoProviderBase : IHTTPBlockInfoProvider
    {
        protected IProviderConfigurationStringProvider _configurationProvider;
        protected IEnumerable<IConfigurationString> _configurationStrings;

        #region Common properties and selectors

        protected string Endpoint
        {
            get
            {
                var valid = new string[] { "Endpoint", "EndpointBase", "BaseURL", "URL" };
                foreach (var configString in _configurationStrings)
                {
                    if (valid.Select(x => x.ToUpper()).Any(x => configString.Name.ToUpper().EndsWith(x)))
                    {
                        return configString.Value;
                    }
                }
                throw new ConfigurationStringNotFoundException("Endpoint", "ETHTPS.Services.Ethereum");
            }
        }

        protected string TXCountSelector => PartialMatchOrThrow("TXCount");
        protected string GasUsedSelector => PartialMatchOrThrow("GasUsed", "Gas");
        protected string DateSelector => PartialMatchOrThrow("DateSelector", "Date");
        protected string BlockHeightSelector => PartialMatchOrThrow("BlockHeight");
        protected string APIKey => PartialMatchOrThrow("APIKey");
        protected string Secret => PartialMatchOrThrow("Secret");
        protected string ProjectID => PartialMatchOrThrow("ProjectID");

        private string PartialMatchOrThrow(params string[] partialNames)
        {
            foreach (var configString in _configurationStrings)
            {
                if (partialNames.Select(x => x.ToUpper()).Any(partialName => configString.Name.ToUpper().Contains(partialName.ToUpper())))
                {
                    return configString.Value;
                }
            }
            throw new ConfigurationStringNotFoundException(string.Join(", ", partialNames), "ETHTPS.Services.Ethereum");
        }

        #endregion

        protected BlockInfoProviderBase(IDBConfigurationProvider configurationProvider, string providerName)
        {
            _configurationStrings = configurationProvider.GetConfigurationStringsForProvider(providerName);
        }

        public abstract double BlockTimeSeconds { get; set; }

        public abstract Task<Block> GetBlockInfoAsync(int blockNumber);
        public abstract Task<Block> GetBlockInfoAsync(DateTime time);
        public abstract Task<Block> GetLatestBlockInfoAsync();
    }
}