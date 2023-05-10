using System;
using System.Text;

using ETHTPS.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    public abstract class InfuraBlockInfoProviderBase : JSONRPCBlockInfoProviderBase
    {
        protected InfuraBlockInfoProviderBase(IDBConfigurationProvider configurationProvider, string providerName) : base(configurationProvider, providerName)
        {
            var authenticationString = $"{ProjectID}:{Secret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
            Endpoint = GetEndpoint(providerName);
            _httpClient.BaseAddress = new Uri($"{Endpoint}/{ProjectID}");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
        }

        private string GetEndpoint(string providerName)
        {
            switch (providerName)
            {
                case "Ethereum":
                    return "https://mainnet.infura.io/v3";
                case "Palm":
                    return "https://palm-mainnet.infura.io/v3";
                case "Optimism":
                    return "https://optimism-mainnet.infura.io/v3";
                case "NEAR":
                    return "https://near-mainnet.infura.io/v3";
                case "Starknet":
                    return "https://starknet-mainnet.infura.io/v3";
                case "Aurora":
                    return "https://aurora-mainnet.infura.io/v3";
                case "Celo":
                    return "https://celo-mainnet.infura.io/v3";
                case "Polygon":
                    return "https://polygon-mainnet.infura.io/v3";
                case "Arbitrum One":
                    return "https://arbitrum-mainnet.infura.io/v3";
                case "AVAX C-chain":
                    return "https://avalanche-mainnet.infura.io/v3";
                default:
                    throw new ArgumentException($"Unknown provider name: {providerName}");
            }
        }
    }
}
