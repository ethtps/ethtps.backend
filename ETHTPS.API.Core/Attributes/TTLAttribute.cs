namespace ETHTPS.API.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TTLAttribute : Attribute
    {
        public TTLAttribute(int ttlSeconds)
        {
            TTL = TimeSpan.FromSeconds(ttlSeconds);
        }

        public TimeSpan TTL { get; private set; }
    }
}
