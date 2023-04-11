namespace ETHTPS.Services.Integration.Cache
{
    public interface ICacheManager<T>
    {
        void Set(string key, T value);
        T? Get(string key);
        void Remove(string key);
    }
}
