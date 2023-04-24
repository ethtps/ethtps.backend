using ETHTPS.Configuration.Database.Initialization;

namespace ETHTPS.Configuration.Database
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
