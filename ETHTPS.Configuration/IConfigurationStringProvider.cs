namespace ETHTPS.Configuration
{
    public interface IConfigurationStringProvider
    {
        IEnumerable<IConfigurationString>? GetConfigurationStrings(string name);
    }
}
