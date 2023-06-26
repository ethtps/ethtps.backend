using ETHTPS.Core;

namespace ETHTPS.Configuration
{
    public interface IMicroservice : ICachedKey
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
