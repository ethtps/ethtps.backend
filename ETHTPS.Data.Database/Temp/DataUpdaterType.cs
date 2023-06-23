using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class DataUpdaterType
    {
        public DataUpdaterType()
        {
            DataUpdaters = new HashSet<DataUpdater>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; } = null!;

        public virtual ICollection<DataUpdater> DataUpdaters { get; set; }
    }
}
