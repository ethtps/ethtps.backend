using System.Reflection;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using JsonProperty = Newtonsoft.Json.Serialization.JsonProperty;

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
                options.SerializerSettings.MaxDepth = 2;
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
                options.JsonSerializerOptions.MaxDepth = 2;
            });
            return builder;
        }

        public sealed class NoVirtualPropertiesResolver : DefaultContractResolver
        {
            private readonly List<string> _namesOfVirtualPropsToKeep = new List<string>(new String[] { });

            public NoVirtualPropertiesResolver() { }

            public NoVirtualPropertiesResolver(IEnumerable<string> namesOfVirtualPropsToKeep)
            {
                this._namesOfVirtualPropsToKeep = namesOfVirtualPropsToKeep.Select(x => x.ToLower()).ToList();
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty prop = base.CreateProperty(member, memberSerialization);
                var propInfo = member as PropertyInfo;
                if (propInfo != null)
                {
                    if (propInfo.GetMethod != null)
                        if (propInfo.GetMethod.IsVirtual && !propInfo.GetMethod.IsFinal
                                                         && !_namesOfVirtualPropsToKeep.Contains(propInfo.Name.ToLower()))
                        {
                            prop.ShouldSerialize = obj => false;
                        }
                }
                return prop;
            }
        }
    }
}
