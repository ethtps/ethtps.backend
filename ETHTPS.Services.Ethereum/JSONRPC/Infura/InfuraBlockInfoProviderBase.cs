using ETHTPS.Configuration;

namespace ETHTPS.Services.Ethereum.JSONRPC.Infura
{
    public abstract class InfuraBlockInfoProviderBase : JSONRPCBlockInfoProviderBase
    {
        protected InfuraBlockInfoProviderBase(IDBConfigurationProvider configurationProvider, string providerName) : base(configurationProvider, providerName)
        {
        }
    }
}
