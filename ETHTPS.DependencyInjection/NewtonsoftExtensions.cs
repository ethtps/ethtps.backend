using System.Text.Json;

using ETHTPS.Core;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ETHTPS.API.DependencyInjection
{
    public static class NewtonsoftExtensions
    {
        /// <summary>
        /// Configures NewtonsoftJson with custom settings
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="ignoreVirtualProperties">Whether to ignore virtual properties from serialization in order to avoid potential recursive serialization issues caused by Entity Framework</param>
        /// <returns></returns>
        public static IMvcBuilder ConfigureNewtonsoftJson(this IMvcBuilder builder, bool ignoreVirtualProperties = true)
        {
            builder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.MaxDepth = 10;
                options.SerializerSettings.Formatting = Formatting.None;
                if (ignoreVirtualProperties) options.SerializerSettings.ContractResolver = new NoVirtualPropertiesResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.MaxDepth = 10;
            });
            return builder;
        }
    }
}
