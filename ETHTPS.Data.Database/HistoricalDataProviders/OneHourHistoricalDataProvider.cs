﻿namespace ETHTPS.Data.Integrations.MSSQL.HistoricalDataServices
{
    public sealed class OneHourHistoricalDataProvider : HistoricalTimedTPSAndGasDataProviderBase<TpsandGasDataHour>
    {
        public OneHourHistoricalDataProvider(EthtpsContext context) : base(Core.TimeInterval.OneHour, context, x => x.TpsandGasDataHours, TimeSpan.FromHours(1))
        {

        }
    }
}
