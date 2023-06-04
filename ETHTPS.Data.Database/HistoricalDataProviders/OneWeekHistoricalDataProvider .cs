namespace ETHTPS.Data.Integrations.MSSQL.HistoricalDataProviders
{
    public sealed class OneWeekHistoricalDataProvider : HistoricalTimedTPSAndGasDataProviderBase<TpsandGasDataWeek>
    {
        public OneWeekHistoricalDataProvider(EthtpsContext context) : base(Core.TimeInterval.OneWeek, context, x => x.TpsandGasDataWeeks, TimeSpan.FromDays(7))
        {

        }
    }
}
