using Newtonsoft.Json;

namespace ETHTPS.Services.Infrastructure.Serialization
{
    public static class SerializationExtensions
    {
        public static string SerializeAsJsonWithEmptyArray<T>(this T source)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new NullToEmptyListResolver(),
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Formatting = Formatting.Indented
            };
            return JsonConvert.SerializeObject(source, settings);
        }
    }
}
