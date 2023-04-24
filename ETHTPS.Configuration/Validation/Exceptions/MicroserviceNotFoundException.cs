namespace ETHTPS.Configuration.Validation.Exceptions
{
    public sealed class MicroserviceNotFoundException : Exception
    {
        public string MicroserviceName { get; private set; }

        public MicroserviceNotFoundException(string microserviceName)
        {
            MicroserviceName = microserviceName;
        }
    }
}
