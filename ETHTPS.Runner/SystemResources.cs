namespace ETHTPS.Runner
{
    public class SystemResources
    {
        private const int _PADDING = 7;
        public SystemResource CPU { get; private set; } = new("CPU", "%", _PADDING)
        {
            Max = 100
        };
        public SystemResource MemoryMB { get; private set; } = new("Memory", "MB", _PADDING);
        public SystemResource NetworkMbit { get; set; } = new("Network", "Mb", _PADDING);
    }
}
