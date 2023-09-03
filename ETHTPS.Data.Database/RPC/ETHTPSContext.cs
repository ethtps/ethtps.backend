using ETHTPS.Data.Integrations.MSSQL.RPC;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class EthtpsContext : ETHTPSContextBase
{
    public virtual DbSet<Updater> Updaters { get; set; }

    public virtual DbSet<UpdaterConfiguration> UpdaterConfigurations { get; set; }

    public virtual DbSet<Health> Healths { get; set; }

    public virtual DbSet<Binding> Bindings { get; set; }

    public virtual DbSet<HealthStatus> HealthStatuses { get; set; }
    public virtual DbSet<Endpoint> Endpoints { get; set; }
}