using ETHTPS.Configuration.Database;
using ETHTPS.Data.Core.Models.Configuration;

namespace ETHTPS.Configuration
{
    public interface IDBConfigurationProvider :
        IEnvironmentConfiguration,
        IMicroserviceProvider,
        IMicroserviceConfigurationProvider, IEnvironmentProvider, IProviderConfigurationStringProvider, IConfigurationStringProvider, IDisposable
    {
        IDBConfigurationProvider this[string environment]
        {
            get;
        }
        IEnumerable<AllConfigurationStringsModel> GetAllConfigurationStrings();
    }
}
