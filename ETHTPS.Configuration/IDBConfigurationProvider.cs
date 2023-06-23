﻿using System.ComponentModel.DataAnnotations;

using ETHTPS.Configuration.Database;

namespace ETHTPS.Configuration
{
    /// <summary>
    /// A configuration provider based on an MSSQL database meant to ease management of configuration strings.
    /// </summary>
    /// <seealso cref="ETHTPS.Configuration.IEnvironmentConfiguration" />
    /// <seealso cref="ETHTPS.Configuration.IMicroserviceProvider" />
    /// <seealso cref="ETHTPS.Configuration.Database.IMicroserviceConfigurationProvider" />
    /// <seealso cref="ETHTPS.Configuration.IEnvironmentProvider" />
    /// <seealso cref="ETHTPS.Configuration.IProviderConfigurationStringProvider" />
    /// <seealso cref="ETHTPS.Configuration.IConfigurationStringProvider" />
    /// <seealso cref="System.IDisposable" />
    public interface IDBConfigurationProvider :
        IEnvironmentConfiguration,
        IMicroserviceProvider,
        IMicroserviceConfigurationProvider,
        IEnvironmentProvider,
        IProviderConfigurationStringProvider,
        IConfigurationStringProvider,
        IDisposable
    {
        /// <summary>
        /// Configures this <see cref="IDBConfigurationProvider"/> for the specified environment.
        /// </summary>
        /// <value>
        /// The <see cref="IDBConfigurationProvider"/>.
        /// </value>
        /// <param name="environment">The environment.</param>
        IDBConfigurationProvider this[string environment]
        {
            get;
        }

        /// <summary>
        /// Dumps all the configuration strings in the database. It should *proooobably* not be here but hey, we're the only ones using this app anyway haha;
        /// ha.
        /// </summary>
        IEnumerable<AllConfigurationStringsModel> GetAllConfigurationStrings();

        /// <summary>
        /// Gets all objects that link to the specified configuration string.
        /// </summary>
        /// <param name="configurationStringID">The ID of the target configuration string</param>
        /// <returns></returns>
        ConfigurationStringLinksModel GetAllLinks(int configurationStringID);

        /// <summary>
        /// Adds or updates a configuration string.
        /// </summary>
        /// <param name="configurationString"></param>
        /// <param name="microservice"></param>
        /// <param name="environment"></param>
        /// throws <see cref="ValidationException"/> if the provided parameters are invalid.
        int AddOrUpdateConfigurationString(ConfigurationStringUpdateModel configurationString,
            string? microservice = null, string? environment = null);

        /// <summary>
        /// Links a provider to a configuration string.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="configurationStringName"></param>
        /// <param name="environmentName"></param>
        /// <returns>The SQL return code of the operation</returns>
        int LinkProviderToConfigurationString(string providerName, string configurationStringName,
            string environmentName = Constants.ENVIRONMENT);

        /// <summary>
        /// Links a provider to a configuration string based on their IDs.
        /// </summary>
        /// <param name="providerID"></param>
        /// <param name="configurationStringID"></param>
        /// <param name="environmentName"></param>
        /// <returns>The number of rows affected</returns>
        int LinkProviderToConfigurationString(int providerID, int configurationStringID,
            string environmentName = Constants.ENVIRONMENT);

        /// <summary>
        /// Removes a configuration string link from a provider based on their IDs.
        /// </summary>
        /// <param name="providerID"></param>
        /// <param name="configurationStringID"></param>
        /// <param name="environmentName"></param>
        /// <returns>The number of rows affected</returns>
        int UnlinkProviderFromConfigurationString(int providerID, int configurationStringID,
            string environmentName = Constants.ENVIRONMENT);

        /// <summary>
        /// Clears the Hangfire job queue.
        /// </summary>
        /// <returns></returns>
        int ClearHangfireQueue();
    }
}
