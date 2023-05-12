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
    public class LatestEntryAggregator<TKey, TValue> : IEnumerable<TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> _dictionary;

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
    }
}
