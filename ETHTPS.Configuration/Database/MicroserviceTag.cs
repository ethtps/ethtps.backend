using Newtonsoft.Json;

namespace ETHTPS.Configuration.Database;

public partial class MicroserviceTag
{
    public int Id { get; set; }

    public int MicroserviceId { get; set; }

    public int TagId { get; set; }
    [JsonIgnore]

    public virtual Microservice Microservice { get; set; } = null!;
    [JsonIgnore]
    public virtual Tag Tag { get; set; } = null!;
}
