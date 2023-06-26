using ETHTPS.Configuration;
using ETHTPS.Configuration.Database;
using ETHTPS.Data.Integrations.MSSQL;

using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.Tests.Data.Database
{
    [TestFixture]
    [Category("Database")]
    [Category("MSSQL")]
    [Category("Essential")]
    public class StoredProcedureTests : TestBase
    {
        [Test]
        public async Task InsertNewExperiment_WhenStoredProcedureExists_InsertsNewExperiment()
        {
            // Arrange
            using (var dbContext = ServiceProvider.GetRequiredService<EthtpsContext>())
            {
                int result = await dbContext.InsertNewExperimentAsync(
                    projectId: 2,
                    experimentName: "MyExperiment",
                    experimentDescription: "A sample experiment",
                    targetName: "All desktop",
                    startDate: DateTime.Now,
                    endDate: DateTime.Now.AddDays(30),
                    enabled: true,
                    displayToNPeopleBeforeEnd: 100,
                    considerFinishedAfterTimeoutSeconds: 60);
                Assert.That(dbContext.Experiments.Any(x => x.Name == "MyExperiment"));
                dbContext.Experiments.Remove(dbContext.Experiments.First(x => x.Name == "MyExperiment"));
            }
        }

        [Test]
        public void DeleteAllJobs_DoesNotThrowError()
        {
            using (var dbContext = ServiceProvider.GetRequiredService<EthtpsContext>())
            {
                Assert.DoesNotThrowAsync(async () => await dbContext.DeleteAllJobsAsync());
            }
        }
        [Test]
        public async Task InsertOrUpdateConfigurationStringAsync_InsertsOrUpdateConfigurationString()
        {
            var configurationProvider = ServiceProvider.GetRequiredService<DBConfigurationProviderWithCache>();
            var configurationProviderContext = ServiceProvider.GetRequiredService<ConfigurationContext>();
            string microserviceName = "ETHTPS.Tests";
            string environmentName = "Development";
            string configStringName = "TestConfigString1";
            string configStringValue = "TestValue";

            // Act
            int result = await configurationProviderContext.InsertOrUpdateConfigurationStringAsync(
                microserviceName,
                environmentName,
                configStringName,
                configStringValue);

            // Assert
            Assert.IsTrue(result > 0, "InsertOrUpdateConfigurationString should return a positive result.");
            configurationProviderContext.ConfigurationStrings?.Remove(configurationProviderContext.ConfigurationStrings.First(x => x.Name == configStringName));
        }

        [Test]
        public void InsertNewExperimentTargetAsync_InsertsNewExperimentTarget()
        {
            var configurationProviderContext = ServiceProvider.GetRequiredService<EthtpsContext>();
            // Arrange
            string targetTypeName = "TestTargetType";
            string targetTypeValue = "TestTargetValue";
            try
            {

                // Act
                Assert.DoesNotThrowAsync(async () => await configurationProviderContext.InsertNewExperimentTargetAsync(
                    targetTypeName,
                    targetTypeValue));
            }
            finally
            {

                configurationProviderContext.ExperimentTargets?.Remove(configurationProviderContext.ExperimentTargets.First(x => x.Name == targetTypeName));
            }

        }
    }
}