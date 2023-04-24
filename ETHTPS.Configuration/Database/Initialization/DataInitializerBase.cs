namespace ETHTPS.Configuration.Database.Initialization
{
    public abstract class DataInitializerBase
    {
        protected readonly IDBConfigurationProvider _provider;

        protected DataInitializerBase(IDBConfigurationProvider provider)
        {
            _provider = provider;
        }

        public abstract void Initialize();
    }
}
