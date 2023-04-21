namespace ETHTPS.Configuration.Validation.Exceptions
{
    public class MicroserviceNotFoundException : Exception
    {
        public string MicroserviceName { get; private set; }

        public MicroserviceNotFoundException(string microserviceName)
        {
            MicroserviceName = microserviceName;
        }
    }
}
