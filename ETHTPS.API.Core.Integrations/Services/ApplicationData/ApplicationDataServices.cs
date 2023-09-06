using ETHTPS.Configuration.Database;
using ETHTPS.Data.Core.Models.ExternalWebsites;
using ETHTPS.Data.Core.Models.ResponseModels.SocialMedia;
using ETHTPS.Data.Integrations.MSSQL;
// ReSharper disable NotResolvedInText

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;

//Putting everything in one file for now, but this should be split up into multiple files as functionality grows

public sealed class ProvidersService : EFCoreCRUDServiceBase<ETHTPS.Data.Integrations.MSSQL.Provider>
{
    public ProvidersService(EthtpsContext context) : base(context.Providers, context)
    {
    }
}

public sealed class ProviderTypesService : EFCoreCRUDServiceBase<ETHTPS.Data.Integrations.MSSQL.ProviderType>
{
    public ProviderTypesService(EthtpsContext context) : base(context.ProviderTypes, context)
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
    public ExternalWebsiteCategoryService(EthtpsContext context) : base(context.ExternalWebsiteCategories ?? throw new ArgumentNullException("ExternalWebsiteCateopries"), context)
    {
    }
}

public sealed class MicroservicesService : EFCoreCRUDServiceBase<Microservice>
{
    public MicroservicesService(ConfigurationContext context) : base(context.Microservices ?? throw new ArgumentNullException("Microservices"), context)
    {
    }
}

public sealed class ExperimentTargetsService : EFCoreCRUDServiceBase<ExperimentTarget>
{
    public ExperimentTargetsService(EthtpsContext context) : base(context.ExperimentTargets ?? throw new ArgumentNullException("ExperimentTargets"), context)
    {
    }
}

public sealed class ExperimentTargetTypesService : EFCoreCRUDServiceBase<ExperimentTargetType>
{
    public ExperimentTargetTypesService(EthtpsContext context) : base(context.ExperimentTargetTypes ?? throw new ArgumentNullException("ExperimentTargetTypes"), context)
    {
    }
}

public sealed class ExperimentRunParametersService : EFCoreCRUDServiceBase<ExperimentRunParameter>
{
    public ExperimentRunParametersService(EthtpsContext context) : base(context.ExperimentRunParameters ?? throw new ArgumentNullException("ExperimentRunParameters"), context)
    {
    }
}

public sealed class ExperimentFeedbackService : EFCoreCRUDServiceBase<ExperimentFeedback>
{
    public ExperimentFeedbackService(EthtpsContext context) : base(context.ExperimentFeedbacks ?? throw new ArgumentNullException("ExperimentFeedbacks"), context)
    {
    }
}

//API keys
public sealed class APIKeysService : EFCoreCRUDServiceBase<Apikey>
{
    public APIKeysService(EthtpsContext context) : base(context.Apikeys ?? throw new ArgumentNullException("Apikeys"), context)
    {
    }
}
public sealed class APIKeyGroupsService : EFCoreCRUDServiceBase<ApikeyGroup>
{
    public APIKeyGroupsService(EthtpsContext context) : base(context.ApikeyGroups ?? throw new ArgumentNullException("ApikeyGroup"), context)
    {
    }
}

public sealed class GroupRoleService : EFCoreCRUDServiceBase<GroupRole>
{
    public GroupRoleService(EthtpsContext context) : base(context.GroupRoles ?? throw new ArgumentNullException("GroupRoles"), context)
    {
    }
}

public sealed class GroupService : EFCoreCRUDServiceBase<Group>
{
    public GroupService(EthtpsContext context) : base(context.Groups ?? throw new ArgumentNullException("Groups"), context)
    {
    }
}

public sealed class PermissionService : EFCoreCRUDServiceBase<Permission>
{
    public PermissionService(EthtpsContext context) : base(context.Permissions ?? throw new ArgumentNullException("Permissions"), context)
    {
    }
}

public sealed class PermissionForRoleService : EFCoreCRUDServiceBase<PermissionsForRole>
{
    public PermissionForRoleService(EthtpsContext context) : base(context.PermissionsForRoles ?? throw new ArgumentNullException("PermissionsForRoles"), context)
    {
    }
}

public sealed class RoleService : EFCoreCRUDServiceBase<Role>
{
    public RoleService(EthtpsContext context) : base(context.Roles ?? throw new ArgumentNullException("Roles"), context)
    {
    }
}
