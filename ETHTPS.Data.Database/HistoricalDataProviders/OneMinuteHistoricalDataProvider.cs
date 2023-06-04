namespace ETHTPS.Data.Integrations.MSSQL.HistoricalDataProviders
{
    public sealed class OneMinuteHistoricalDataProvider : HistoricalTimedTPSAndGasDataProviderBase<TpsandGasDataMinute>
    {
        public OneMinuteHistoricalDataProvider(EthtpsContext context) : base(Core.TimeInterval.OneMinute, context, x => x.TpsandGasDataMinutes
        , TimeSpan.FromSeconds(60))
        {

        }
    }
}
