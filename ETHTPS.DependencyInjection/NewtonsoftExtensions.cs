using Microsoft.Extensions.DependencyInjection;

namespace ETHTPS.API.DependencyInjection
{
    public static class NewtonsoftExtensions
    {
        public static IMvcBuilder ConfigureNewtonsoftJson(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.MaxDepth = 2;
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.MaxDepth = 2;
            });
            return builder;
        }
    }
}
