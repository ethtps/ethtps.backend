using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;

public static class ApplicationDataExtensions
{
    public static IServiceCollection AddApplicationDataServices(this IServiceCollection services) =>
        services
            .AddScoped<ProviderTypesService>()
            .AddScoped<ProvidersService>()//Added controller (ac)
            .AddScoped<EnvironmentService>() // (ac)
            .AddScoped<ExternalWebsiteCategoryService>() //(ac)
            .AddScoped<ProviderLinksService>()// (ac)
            .AddScoped<ExperimentTargetTypesService>()
            .AddScoped<ExperimentTargetsService>()
            .AddScoped<ExperimentRunParametersService>()
            .AddScoped<ExperimentFeedbackService>()
            .AddScoped<ExperimentService>()
            .AddScoped<MicroservicesService>()
            .AddScoped<APIKeysService>()
            .AddScoped<APIKeyGroupsService>()
            .AddScoped<GroupRoleService>()
            .AddScoped<GroupService>()
            .AddScoped<PermissionService>()
            .AddScoped<PermissionForRoleService>()
            .AddScoped<RoleService>()
            .AddScoped<ExternalWebsitesService>();// (ac)
}