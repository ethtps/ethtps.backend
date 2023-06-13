namespace ETHTPS.Data.Integrations.MSSQL;

public partial class Job
{
    public long Id { get; set; }

    public long? StateId { get; set; }

    public required string StateName { get; set; }

    public required string InvocationData { get; set; }

    public required string Arguments { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public virtual ICollection<JobParameter> JobParameters { get; } = new List<JobParameter>();

    public virtual ICollection<State> States { get; } = new List<State>();
}
