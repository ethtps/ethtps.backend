namespace ETHTPS.Configuration.Database;

public partial class Provider
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static Provider EMPTY = new Provider();
#pragma warning restore CA2211 // Non-constant fields should not be visible
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public string Color { get; set; } = null!;

    public bool? IsGeneralPurpose { get; set; }

    public int? HistoricalAggregationDeltaBlock { get; set; }

    public bool Enabled { get; set; }

    public int? SubchainOf { get; set; }

    public int TheoreticalMaxTps { get; set; }

    public virtual ICollection<Provider> InverseSubchainOfNavigation { get; } = new List<Provider>();

    public virtual ICollection<ProviderConfigurationString> ProviderConfigurationStrings { get; } = new List<ProviderConfigurationString>();
    public virtual ICollection<ProviderTag> ProviderTags { get; } = new List<ProviderTag>();

    public virtual Provider? SubchainOfNavigation { get; set; } = new();
}
