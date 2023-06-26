using System.Collections;

namespace ETHTPS.Configuration.Extensions;

public static class ReflectionExtensions
{


    /// <summary>
    /// https://stackoverflow.com/a/63794667
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsIEnumerable(this Type type)
    {
        return type.GetInterfaces().Append(type).Any(x => x.IsGenericType && (typeof(IEnumerable<>).IsAssignableFrom(x.GetGenericTypeDefinition()) || x.GetGenericTypeDefinition() == typeof(IEnumerable<>) || x.GetGenericTypeDefinition() == typeof(IEnumerable)));
    }
}