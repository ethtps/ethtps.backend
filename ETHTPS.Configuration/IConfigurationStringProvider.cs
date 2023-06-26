using ETHTPS.Configuration.Database;

namespace ETHTPS.Configuration
{
    public interface IConfigurationStringProvider
    {

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
        IEnumerable<IConfigurationString>? GetConfigurationStrings(string name);
    }
}
