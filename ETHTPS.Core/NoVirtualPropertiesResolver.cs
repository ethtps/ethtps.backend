using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using JsonProperty = Newtonsoft.Json.Serialization.JsonProperty;

namespace ETHTPS.Core
{
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
