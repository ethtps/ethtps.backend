namespace ETHTPS.Data.Integrations.MSSQL.HistoricalDataProviders
{
    public sealed class OneMonthHistoricalDataProvider : HistoricalTimedTPSAndGasDataProviderBase<TpsandGasDataMonth>
    {
        public OneMonthHistoricalDataProvider(EthtpsContext context) : base(Core.TimeInterval.OneMonth, context, x => x.TpsandGasDataMonths, TimeSpan.FromDays(30))
        {

        }
    }
}
