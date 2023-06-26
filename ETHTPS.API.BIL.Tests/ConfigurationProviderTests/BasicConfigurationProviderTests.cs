using ETHTPS.Configuration;
using ETHTPS.Configuration.Database;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.ConfigurationProviderTests
{
    /// <summary>
    /// Long live ChatGPT. Some of these test cases didn't even cross my mind
    /// </summary>
    [TestFixture]
    [Category("Configuration")]
    [Category("Essential")]
    public sealed class BasicConfigurationProviderTests : TestBase
    {

        [SetUp]
        public void Setup()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void DependencyInjectionTest()
        {
            Assert.NotNull(ServiceProvider.GetRequiredService<DBConfigurationProviderWithCache>());
            Assert.Pass();
        }

        [Test]
        public void ReturnsHangfireConnectionString()
        {
            var strings = ConfigurationProvider.GetConfigurationStringsForMicroservice("ETHTPS.TaskRunner");
            if (strings == null)
            {
                Assert.That(strings, Is.Not.Null);
                return;
            }
            strings = strings.ToList();
            Assert.That(strings.Count(), Is.GreaterThan(0));
            Assert.That(strings.Count(x => x.Name == "HangfireConnectionString"), Is.GreaterThan(1));
        }

        [Test]
        public void AddEnvironments_WithNewEnvironment_AddsNewEnvironment()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<DBConfigurationProviderWithCache>();
            var environmentName = "TestEnvironment";

            // Act
            provider.AddEnvironments(environmentName);

            // Assert
            var environment = ServiceProvider.GetRequiredService<ConfigurationContext>().Environments?.SingleOrDefault(x => x.Name == environmentName);
            Assert.That(environment, Is.Not.Null);
        }

        [Test]
        public void AddEnvironments_WithExistingEnvironment_DoesNotAddNewEnvironment()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<DBConfigurationProviderWithCache>();
            var environmentName = "Development";

            // Act
            provider.AddEnvironments(environmentName);

            // Assert
            var environments = ServiceProvider.GetRequiredService<ConfigurationContext>().Environments?.Where(x => x.Name == environmentName);
            Assert.That(environments?.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetEnvironmentID_WithExistingEnvironment_ReturnsCorrectID()
        {
            // Arrange
            var provider = ServiceProvider.GetRequiredService<DBConfigurationProviderWithCache>();
            var environmentName = "Development";
            var expectedEnvironmentID = ServiceProvider.GetRequiredService<ConfigurationContext>().Environments?.Single(x => x.Name == environmentName).Id;

            // Act
            var environmentID = provider.GetEnvironmentID(environmentName);

            // Assert
            Assert.That(environmentID, Is.EqualTo(expectedEnvironmentID));
        }
    }
}
