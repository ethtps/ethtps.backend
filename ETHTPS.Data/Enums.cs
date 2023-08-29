
using ETHTPS.Data.Core.Attributes;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ETHTPS.Data.Core
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TimeInterval
    {
        [GroupBy(TimeGrouping.Second)] Instant,
        [GroupBy(TimeGrouping.Second)] OneMinute,
        [GroupBy(TimeGrouping.Minute)] OneHour,
        [GroupBy(TimeGrouping.Hour)] OneDay,
        [GroupBy(TimeGrouping.Day)] OneWeek,
        [GroupBy(TimeGrouping.Day)] OneMonth,
        [GroupBy(TimeGrouping.Month)] OneYear,
        [GroupBy(TimeGrouping.Year)] All,
        [GroupBy(TimeGrouping.Auto)] Auto
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TimeGrouping
    {
        Second,
        Minute,
        Hour,
        Day,
        Week,
        DayOfWeek,
        Month,
        Year,
        Decade,
        Auto
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataType
    {
        TPS, GPS, GasAdjustedTPS
    }

    /// <summary>
    /// A list of ETHTPS microservices or libraries that have their own configuration and permissions.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Microservice
    {
        [FullName("ETHTPS.API.General")]
        API,
        [FullName("ETHTPS.TaskRunner")]
        TaskRunner,
        [FullName("ETHTPS.Admin.Backend")]
        Admin,
        [FullName("ETHTPS.WSAPI")]
        WSAPI,
        [FullName("ETHTPS.Configuration.IDBConfigurationProvider")]
        Configuration,
        [FullName("ETHTPS.Tests")]
        Tests,
        [FullName("ETHTPS.Services")]
        Services,
        /// <summary>
        /// Dummy value, shouldn't really be used or even needed anywhere.
        /// </summary>
        [FullName("ETHTPS.TestsAPI")]
        Mock
    }
}
