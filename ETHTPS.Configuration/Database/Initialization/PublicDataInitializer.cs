namespace ETHTPS.Configuration.Database.Initialization
{
    internal sealed class PublicDataInitializer : DataInitializerBase
    {
        public PublicDataInitializer(IDBConfigurationProvider provider) : base(provider)
        {

        }

        public override void Initialize()
        {
            _provider.AddEnvironments("Debug", "Staging", "Release", "All");

        }
    }
}
