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
    }
}
