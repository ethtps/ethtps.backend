namespace ETHTPS.Data.Integrations.MSSQL
{
    public class TimedTPSAndGasData : TPSAndGasDataBase
    {
        public DateTime StartDate { get; set; }
        public double AverageTps { get; set; }
        public double AverageGps { get; set; }
        public int ReadingsCount { get; set; }
        //public string OclhJson { get; set; }
    }

    public sealed class TpsandGasDataDay : TimedTPSAndGasData { }
    public sealed class TpsandGasDataHour : TimedTPSAndGasData { }
    public sealed class TpsandGasDataWeek : TimedTPSAndGasData { }
    public sealed class TpsandGasDataMinute : TimedTPSAndGasData { }
    public sealed class TpsandGasDataMonth : TimedTPSAndGasData { }
    public sealed class TpsandGasDataAll : TimedTPSAndGasData { }
    public sealed class TpsandGasDataYear : TimedTPSAndGasData { }
}
