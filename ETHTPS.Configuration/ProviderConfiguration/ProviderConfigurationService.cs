using ETHTPS.Configuration.Database;

namespace ETHTPS.Configuration.ProviderConfiguration
{
    public sealed class ProviderConfigurationService : IProviderConfigurationService
    {
        private readonly ConfigurationContext _dbContext;

        public ProviderConfigurationService(ConfigurationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(string providerName, string configurationStringName, string environmentName)
        {
            var provider = _dbContext.Providers.FirstOrDefault(p => p.Name == providerName);
            if (provider == null)
            {
                // handle provider not found error
                return;
            }

            var configurationString = _dbContext.ConfigurationStrings.FirstOrDefault(cs => cs.Name == configurationStringName);
            if (configurationString == null)
            {
                // handle configuration string not found error
                return;
            }

            var environment = _dbContext.Environments.FirstOrDefault(e => e.Name == environmentName);
            if (environment == null)
            {
                // handle environment not found error
                return;
            }

            var providerConfigurationString = new ProviderConfigurationString
            {
                Provider = provider,
                ConfigurationString = configurationString,
                Environment = environment
            };

            _dbContext.ProviderConfigurationStrings.Add(providerConfigurationString);
            _dbContext.SaveChanges();
        }

        public ProviderConfigurationString? Get(string providerName, string configurationStringName, string environmentName)
        {
            return _dbContext.ProviderConfigurationStrings.FirstOrDefault(pcs =>
                pcs.Provider.Name == providerName &&
                pcs.ConfigurationString.Name == configurationStringName &&
                pcs.Environment.Name == environmentName);
        }

        public void Delete(string providerName, string configurationStringName, string environmentName)
        {
            var providerConfigurationString = _dbContext.ProviderConfigurationStrings.FirstOrDefault(pcs =>
                pcs.Provider.Name == providerName &&
                pcs.ConfigurationString.Name == configurationStringName &&
                pcs.Environment.Name == environmentName);

            if (providerConfigurationString != null)
            {
                _dbContext.ProviderConfigurationStrings.Remove(providerConfigurationString);
                _dbContext.SaveChanges();
            }
        }
    }
}
