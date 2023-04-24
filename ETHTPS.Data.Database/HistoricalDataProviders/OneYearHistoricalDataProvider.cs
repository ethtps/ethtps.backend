namespace ETHTPS.Data.Integrations.MSSQL.HistoricalDataServices
{
    public sealed class OneYearHistoricalDataProvider : HistoricalTimedTPSAndGasDataProviderBase<TpsandGasDataYear>
    {
        public OneYearHistoricalDataProvider(EthtpsContext context) : base(Core.TimeInterval.OneYear, context, x => x.TpsandGasDataYears, TimeSpan.FromDays(365))
        {

        }
    }
}
