namespace ETHTPS.Data.Core.Models.Providers.Markdown
{
    public interface IMarkdownPage : IIndexed
    {
        public string RawMarkdown { get; set; }
    }
}
