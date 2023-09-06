using ETHTPS.Data.Core;

namespace ETHTPS.Configuration.Database;

public partial class Microservice : IMicroservice, IIndexed
{
    public static Microservice From(IMicroservice microservice) => new()
    {
        Name = microservice.Name,
        Description = microservice.Description,
    };
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static Microservice EMPTY = new() { Name = "" };
#pragma warning restore CA2211 // Non-constant fields should not be visible
    public int Id { get; set; }
    public virtual ICollection<MicroserviceConfigurationString> MicroserviceConfigurationStrings { get; } = new List<MicroserviceConfigurationString>();
    public virtual ICollection<MicroserviceTag> MicroserviceTags { get; } = new List<MicroserviceTag>();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string ToCacheKey() => $"{Id}{Name}";
}
