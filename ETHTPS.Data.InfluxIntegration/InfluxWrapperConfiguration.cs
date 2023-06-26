using ETHTPS.Configuration;
using ETHTPS.Configuration.Extensions;
using ETHTPS.Configuration.Validation.Exceptions;

namespace ETHTPS.Data.Integrations.InfluxIntegration
{
    public sealed class InfluxWrapperConfiguration
    {
        public string? URL { get; set; }
        public string? Bucket { get; set; }
        public string? Org { get; set; }
        public string? OrgID { get; set; }
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public static InfluxWrapperConfiguration FromConfigurationProvider(DBConfigurationProviderWithCache configurationProvider)
        {
            string GetConfigurationString(string key) => configurationProvider.GetFirstConfigurationString(key) ?? throw new ConfigurationStringNotFoundException(key, DBConfigurationProviderWithCache.EntryAppName ?? "ETHTPS.Data.Integrations.InfluxIntegration");
            return new InfluxWrapperConfiguration()
            {
                Bucket = GetConfigurationString("InfluxDB_prod_bucket"),
                URL = GetConfigurationString("InfluxDB_prod_url"),
                Org = GetConfigurationString("InfluxDB_prod_org"),
                Token = GetConfigurationString("InfluxDB_token"),
                Username = GetConfigurationString("InfluxDB_prod_user"),
                Password = GetConfigurationString("InfluxDB_prod_password"),
                OrgID = GetConfigurationString("InfluxDB_prod_orgid")
            };
        }
    }
}
