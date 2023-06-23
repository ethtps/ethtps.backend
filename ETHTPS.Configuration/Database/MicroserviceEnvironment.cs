namespace ETHTPS.Configuration.Database;

public sealed class MicroserviceEnvironment
{
    public required string Microservice { get; set; }
    public required string Environment { get; set; }
}