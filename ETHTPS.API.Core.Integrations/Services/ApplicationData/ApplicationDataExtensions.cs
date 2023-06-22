using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData;

public static class ApplicationDataExtensions
{
    public static IServiceCollection AddApplicationDataServices(this IServiceCollection services) =>
        services.AddScoped<ProvidersService>()//Added controller (ac)
            .AddScoped<EnvironmentService>() // (ac)
            .AddScoped<ExternalWebsiteCategoryService>() //(ac
            .AddScoped<ProviderLinksService>()
            .AddScoped<ExternalWebsitesService>();// (ac)
}