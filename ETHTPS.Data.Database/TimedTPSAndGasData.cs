namespace ETHTPS.Data.Integrations.MSSQL
{
    public partial class TimedTPSAndGasData : TPSAndGasDataBase
    {
        public DateTime StartDate { get; set; }
        public double AverageTps { get; set; }
        public double AverageGps { get; set; }
        public int ReadingsCount { get; set; }
        //public string OclhJson { get; set; }
    }

    public partial class TpsandGasDataDay : TimedTPSAndGasData { }
    public partial class TpsandGasDataHour : TimedTPSAndGasData { }
    public partial class TpsandGasDataWeek : TimedTPSAndGasData { }
    public partial class TpsandGasDataMinute : TimedTPSAndGasData { }
    public partial class TpsandGasDataMonth : TimedTPSAndGasData { }
    public partial class TpsandGasDataAll : TimedTPSAndGasData { }
    public partial class TpsandGasDataYear : TimedTPSAndGasData { }
}
