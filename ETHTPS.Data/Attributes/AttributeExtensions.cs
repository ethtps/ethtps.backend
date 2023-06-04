using System;
using System.Linq;

using ETHTPS.Data.Core.BlockInfo;

namespace ETHTPS.Data.Core.Attributes
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// Returns the <see cref="TimeInterval"/> that is associated with this value or throws an exception if one was not found
        /// </summary>
        public static TimeGrouping ExtractTimeGrouping(this TimeInterval timeInterval) => timeInterval.GetAttribute<GroupByAttribute>().Grouping;

        /// <summary>
        /// https://stackoverflow.com/a/19621488
        /// </summary>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0
              ? (T)attributes[0]
              : null;
        }
        public static string GetProviderName<T>(this T blockInfoProviderType)
            where T : IHTTPBlockInfoProvider => typeof(T).GetProviderName();

        public static string GetProviderName(this Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(ProviderAttribute), true);
            if (attributes.Any())
            {
                var attribute = attributes.First();
                return (attribute as ProviderAttribute).Name;
            }
            else
            {
                throw new ArgumentNullException($"Provider {type} isn't marked with any {typeof(ProviderAttribute)}");
            }
        }

        public static string GetProviderNameFromFirstGenericArgument(this Type type)
        {
            if (type.GetGenericArguments().Length > 0)
            {
                var attributes = type.GetGenericArguments().First().GetCustomAttributes(typeof(ProviderAttribute), true);
                if (attributes.Any())
                {
                    var attribute = attributes.First();
                    return (attribute as ProviderAttribute).Name;
                }
            }
            return string.Empty;
        }
    }
}
