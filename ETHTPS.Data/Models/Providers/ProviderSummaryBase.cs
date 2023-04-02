namespace ETHTPS.Data.Core.Models.Providers
{
    public abstract class ProviderSummaryBase : IProvider
    {

        public string Name { get; set; }
        public int Id { get; set; }
    }
}
