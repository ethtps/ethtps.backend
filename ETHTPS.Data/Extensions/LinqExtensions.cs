using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ETHTPS.Data.Core.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Selects only the non-null elements of this list and returns an empty list if none is found
        /// </summary>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source) => source.SafeWhere(x => x != null);

        public static IEnumerable<T> Where2<T>(this IEnumerable<T> source, Func<T, int> predicate) => Enumerable.Where(source, x => predicate(x) == 1);
#pragma warning disable SYSLIB0011
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
#pragma warning restore SYSLIB0011
        //public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate) => source;

        public static T FirstIfAny<T>(this IEnumerable<T> source, Func<T, bool> selector)
        {
            try
            {
                if (source == null || !source.Any())
                    return default(T);

                if (source.Any(selector))
                {
                    return source.First(selector);
                }
            }
            catch { }
            return default(T);
        }

        /// <summary>
        /// A variant of <see cref="System.Linq.Enumerable.Where{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/> that returns an empty array instead of throwing an exception when the search clause finds no results or the source is null.
        /// </summary>
        public static IEnumerable<T> SafeWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null || !source.Any(predicate))
                return Enumerable.Empty<T>();

            return source.Where(predicate);
        }

        /// <summary>
        /// A variant of <see cref="System.Linq.Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, int, TResult}){TSource}(IEnumerable{TSource}, Func{TSource, bool})"/> that returns an empty array instead of throwing an exception when the search clause finds no results or the source is null.
        /// </summary>
        public static IEnumerable<TReturn> SafeSelect<T, TReturn>(this IEnumerable<T> source, Func<T, TReturn> selector)
        {
            if (source == null || !source.Any())
                return Enumerable.Empty<TReturn>();

            return source.Select(selector);
        }

        public static bool SafeAny<T>(this IEnumerable<T>? source, Func<T, bool> selector)
        {
            try
            {
                if (source == null || !source.Any(selector))
                    return false;

                return source?.Count(x => x != null) > 0;
            }
            catch { }
            return false;
        }
    }
}
