using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ETHTPS.Services.LiveData
{
    /// <summary>
    /// Represents a collection of values, where each key can only have one value, and the latest value is always the one that is kept
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    public class LatestEntryAggregator<TKey, TValue> : IEnumerable<TValue>, IDisposable
    {
        private readonly ConcurrentDictionary<TKey, TValue> _dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="LatestEntryAggregator{TKey, TValue}"/> class using a source collection.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        public LatestEntryAggregator(IEnumerable<TValue> source, Func<TValue, TKey> keySelector) : this()
        {
            Push(source, keySelector);
        }

        public LatestEntryAggregator()
        {
            _dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        /// <summary>
        ///  Pushes a new value to the collection, if the key already exists, the value is overwritten
        /// </summary>
        public void Push(TKey key, TValue value)
        {
            _dictionary.AddOrUpdate(key, value, (k, v) => value);
        }

        public void Push(IEnumerable<TValue> source, Func<TValue, TKey> keySelector)
        {
            foreach (var item in source) Push(keySelector(item), item);
        }

        /// <summary>
        /// Clears the collection
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TValue> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Determines the difference between this <see cref="LatestEntryAggregator{TKey, TValue}"/> and the provided one; values for which there's a new key or for which the selector returns true are returned. Similar to a set difference operation in math (non-commutative; this method returns [result = (other - this)]).
        /// </summary>
        public IEnumerable<TValue> Diff(LatestEntryAggregator<TKey, TValue> other, Func<TValue, TValue, bool> diffKeySelector)
        {
            var result = new List<TValue>();

            foreach (var key in other._dictionary.Keys)
            {
                if (!_dictionary.ContainsKey(key))
                {
                    result.Add(other._dictionary[key]);
                }
                else if (diffKeySelector(_dictionary[key], other._dictionary[key]))
                {
                    result.Add(other._dictionary[key]);
                }
            }

            return result;
        }

        public void Dispose()
        {
            _dictionary?.Clear();
        }
    }
}
