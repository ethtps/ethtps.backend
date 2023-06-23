namespace ETHTPS.Data.Core
{
    public interface ICachedKey
    {
        /// <summary>
        /// Returns a key that can be used to cache this object.
        /// </summary>
        public string ToCacheKey();
    }
}
