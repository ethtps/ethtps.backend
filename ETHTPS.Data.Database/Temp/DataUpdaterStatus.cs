using System;
using System.Collections.Generic;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class DataUpdaterStatus
    {
        public DataUpdaterStatus()
        {
            LiveDataUpdaterStatuses = new HashSet<LiveDataUpdaterStatus>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<LiveDataUpdaterStatus> LiveDataUpdaterStatuses { get; set; }
    }
}
