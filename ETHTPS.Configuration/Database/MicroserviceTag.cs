namespace ETHTPS.Configuration.Database;

public partial class MicroserviceTag
{
    public int Id { get; set; }

    public int MicroserviceId { get; set; }

    public int TagId { get; set; }

    public virtual Microservice Microservice { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
