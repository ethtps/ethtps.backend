﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.API.Security.Core.Policies
{
    public static class CORSPolicies
    {
        private static readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public static IServiceCollection AddCustomCORSPolicies(this IServiceCollection services) => services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("https://ethtps.info");
                          builder.WithOrigins("https://v2.ethtps.info");
                          builder.WithOrigins("https://alpha.ethtps.info");
                          builder.WithOrigins("https://beta.ethtps.info");
                          builder.WithOrigins("https://ultrasound.money/");
                          builder.WithOrigins("http://localhost:3007");
                          builder.AllowAnyHeader();
                      });
        });

        public static IApplicationBuilder UseCustomCORSPolicies(this IApplicationBuilder app) => app.UseCors(MyAllowSpecificOrigins);
    }
}
