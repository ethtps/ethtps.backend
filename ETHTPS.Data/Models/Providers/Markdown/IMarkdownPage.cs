namespace ETHTPS.Data.Core.Models.Markdown
{
    public interface IMarkdownPage : IIndexed
    {
        public string RawMarkdown { get; set; }
    }
}
