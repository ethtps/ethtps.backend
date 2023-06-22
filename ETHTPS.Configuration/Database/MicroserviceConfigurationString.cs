using Newtonsoft.Json;

namespace ETHTPS.Configuration.Database;

public partial class MicroserviceConfigurationString
{
    public int Id { get; set; }

    public int MicroserviceId { get; set; }

    public int ConfigurationStringId { get; set; }

    public int EnvironmentId { get; set; }
    [JsonIgnore]
    public virtual ConfigurationString? ConfigurationString { get; set; } = null!;
    [JsonIgnore]
    public virtual Environment? Environment { get; set; } = null!;
    [JsonIgnore]

    public virtual Microservice? Microservice { get; set; } = null!;
}
