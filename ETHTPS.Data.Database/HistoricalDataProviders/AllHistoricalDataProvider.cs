namespace ETHTPS.Data.Integrations.MSSQL.HistoricalDataServices
{
    public sealed class AllHistoricalDataProvider : HistoricalTimedTPSAndGasDataProviderBase<TpsandGasDataAll>
    {
        public AllHistoricalDataProvider(EthtpsContext context) : base(Core.TimeInterval.All, context, x => x.TpsandGasDataAlls, TimeSpan.MaxValue)
        {

        }
    }
}
