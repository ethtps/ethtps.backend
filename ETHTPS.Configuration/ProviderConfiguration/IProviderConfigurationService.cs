using ETHTPS.Configuration.Database;

namespace ETHTPS.Configuration.ProviderConfiguration
{
    public interface IProviderConfigurationService
    {
        void Add(string providerName, string configurationStringName, string environmentName);
        ProviderConfigurationString? Get(string providerName, string configurationStringName, string environmentName);
        void Delete(string providerName, string configurationStringName, string environmentName);
    }
}
