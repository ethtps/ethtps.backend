namespace ETHTPS.Data.Integrations.MSSQL.HistoricalDataProviders
{
    public sealed class OneHourHistoricalDataProvider : HistoricalTimedTPSAndGasDataProviderBase<TpsandGasDataHour>
    {
        public OneHourHistoricalDataProvider(EthtpsContext context) : base(Core.TimeInterval.OneHour, context, x => x.TpsandGasDataHours, TimeSpan.FromHours(1))
        {

        }
    }
}
