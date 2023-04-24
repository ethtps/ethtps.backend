using ETHTPS.API.BIL.Infrastructure.Services;
using ETHTPS.Data.Core.Extensions.StringExtensions;
using ETHTPS.Data.Core.Models.Markdown;
using ETHTPS.Data.Integrations.MSSQL;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services
{
    public class MarkdownService : EFCoreCRUDServiceBase<MarkdownPage>, IMarkdownService<MarkdownPage>
    {
        private readonly EthtpsContext _context;
        public MarkdownService(EthtpsContext context) : base(context.MarkdownPages, context)
        {
            _context = context;
        }

        public IEnumerable<IMarkdownPage> GetMarkdownPagesFor(string providerName)
        {
            var result = _context.ProviderDetailsMarkdownPages?.ToList()?.Where(x => x.Provider.Name.LossyCompareTo(providerName)).Select(x => x.MarkdownPage);
            if (result == null)
                throw new KeyNotFoundException(providerName);
            return result;
        }
    }
}
