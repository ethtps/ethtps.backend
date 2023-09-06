namespace ETHTPS.API.BIL.Infrastructure.Services.DataUpdater
{
    /// <summary>
    /// What type of runner should we use?
    /// </summary>
    public enum BackgroundServiceType
    {
        /// <summary>
        /// This one works fine if using a database but it fills up the logs over time and makes it crash.
        /// </summary>
        Hangfire,
        /// <summary>
        /// This one's not robust and for some reason won't work though it used to imerc ???
        /// </summary>
        Coravel
    }

    public enum DatabaseProvider { MSSQL, InfluxDB, InMemory }
}
