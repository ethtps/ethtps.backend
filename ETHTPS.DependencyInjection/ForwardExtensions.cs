using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace ETHTPS.API.DependencyInjection
{
    public static class ForwardExtensions
    {
        public static IApplicationBuilder RequestsAreForwardedByReverseProxy(this IApplicationBuilder app) => app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
    }
}