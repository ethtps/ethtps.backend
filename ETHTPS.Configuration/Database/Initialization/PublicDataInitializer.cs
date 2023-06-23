namespace ETHTPS.Configuration.Database.Initialization
{
    public sealed class PublicDataInitializer : DataInitializerBase
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
