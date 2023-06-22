namespace ETHTPS.Configuration.Database;

public partial class ProviderTag
{
    public int Id { get; set; }

    public int ProviderId { get; set; }

    public int TagId { get; set; }

    public virtual Provider Provider { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
