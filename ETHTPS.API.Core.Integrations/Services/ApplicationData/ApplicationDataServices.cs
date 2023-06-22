using ETHTPS.Configuration.Database;
using ETHTPS.Data.Core.Models.ExternalWebsites;
using ETHTPS.Data.Core.Models.ResponseModels.SocialMedia;
using ETHTPS.Data.Integrations.MSSQL;
// ReSharper disable NotResolvedInText

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;

public sealed class ProvidersService : EFCoreCRUDServiceBase<ETHTPS.Data.Integrations.MSSQL.Provider>
{
    public ProvidersService(EthtpsContext context) : base(context.Providers, context)
    {
    }
}

public sealed class NetworksService : EFCoreCRUDServiceBase<ETHTPS.Data.Integrations.MSSQL.Network>
{
    public NetworksService(EthtpsContext context) : base(context.Networks, context)
    {
    }
}
public sealed class EnvironmentService : EFCoreCRUDServiceBase<ETHTPS.Configuration.Database.Environment>
{
    public EnvironmentService(ConfigurationContext context) : base(context.Environments ?? throw new ArgumentNullException("Environments"), context)
    {
    }
}

public sealed class ExternalWebsitesService : EFCoreCRUDServiceBase<ExternalWebsite>
{
    private readonly EthtpsContext _context;
    public ExternalWebsitesService(EthtpsContext context) : base(context.ExternalWebsites, context)
    {
        _context = context;
    }

    public IEnumerable<IProviderExternalWebsite> GetExternalWebsitesFor(string providerName)
    {
        lock (_context.LockObj)
        {
            var links = _context.ProviderLinks.ToList()
                .Where(x => x.Provider != null ? x.Provider.Name == providerName : false)
                .AsEnumerable();
            return links
                .Select(link => new ProviderExternalWebsite()
                {
                    Category = link.ExternalWebsite?.CategoryNavigation?.Id ?? 1,
                    Name = link.ExternalWebsite?.Name,
                    IconBase64 = link.ExternalWebsite?.IconBase64.Length == 0 ? null : link.ExternalWebsite?.IconBase64,
                    Url = link.Link
                });
        }
    }
}

public sealed class ExternalWebsiteCategoryService : EFCoreCRUDServiceBase<ExternalWebsiteCategory>
{
    public ExternalWebsiteCategoryService(EthtpsContext context) : base(context.ExternalWebsiteCateopries ?? throw new ArgumentNullException("ExternalWebsiteCateopries"), context)
    {
    }
}

public sealed class ProviderLinksService : EFCoreCRUDServiceBase<ProviderLink>
{
    public ProviderLinksService(EthtpsContext context) : base(context.ProviderLinks ?? throw new ArgumentNullException("ProviderLinks"), context)
    {
    }
}