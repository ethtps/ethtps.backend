namespace ETHTPS.Data.Integrations.MSSQL;

public partial class MarkdownPage
{
    public int Id { get; set; }

    public string RawMarkdown { get; set; } = null!;

    public virtual ICollection<ProviderDetailsMarkdownPage> ProviderDetailsMarkdownPages { get; set; } = new List<ProviderDetailsMarkdownPage>();
}
