namespace ETHTPS.Configuration.Database;

public partial class ConfigurationStringTag
{
    public int Id { get; set; }

    public int ConfigurationStringId { get; set; }

    public int TagId { get; set; }

    public virtual ConfigurationString ConfigurationString { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
