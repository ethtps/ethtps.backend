using ETHTPS.Configuration.Database;

using ETHTPS.Configuration.ProviderConfiguration;

using Microsoft.Extensions.DependencyInjection;

using Environment = ETHTPS.Configuration.Database.Environment;

namespace ETHTPS.Tests.ConfigurationProviderTests
{
    [TestFixture]
    public sealed class ProviderConfigurationServiceTests : TestBase
    {

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void NotNullTest()
        {
            Assert.NotNull(ServiceProvider.GetRequiredService<ProviderConfigurationService>());
        }

        [Test]
        public void AddGetDelete_ConfigurationValue_Success()
        {
            var _dbContext = ServiceProvider.GetRequiredService<ConfigurationContext>();
            // Arrange
            var providerName = "TestProvider";
            var configurationName = "TestConfiguration";
            var environmentName = "TestEnvironment";

            var provider = new Provider { Name = providerName };
            var configurationString = new ConfigurationString { Name = configurationName };
            var environment = new Environment { Name = environmentName };

            _dbContext.Providers.Add(provider);
            _dbContext.ConfigurationStrings.Add(configurationString);
            _dbContext.Environments.Add(environment);
            _dbContext.SaveChanges();

            var providerConfigurationService = ServiceProvider.GetRequiredService<IProviderConfigurationService>();

            // Act
            providerConfigurationService.Add(providerName, configurationName, environmentName);

            var providerConfigurationString = providerConfigurationService.Get(providerName, configurationName, environmentName);

            providerConfigurationService.Delete(providerName, configurationName, environmentName);

            var deletedProviderConfigurationString = providerConfigurationService.Get(providerName, configurationName, environmentName);

            // Assert
            Assert.NotNull(providerConfigurationString);
            Assert.AreEqual(providerName, providerConfigurationString.Provider.Name);
            Assert.AreEqual(configurationName, providerConfigurationString.ConfigurationString.Name);
            Assert.AreEqual(environmentName, providerConfigurationString.Environment.Name);

            Assert.Null(deletedProviderConfigurationString);
        }


    }
}
