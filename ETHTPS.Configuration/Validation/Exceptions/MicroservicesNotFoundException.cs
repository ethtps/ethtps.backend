namespace ETHTPS.Configuration.Validation.Exceptions
{
    public class MicroservicesNotFoundException : Exception
    {
        public string[] Microservices { get; private set; }

        public MicroservicesNotFoundException(string[] microservices)
        {
            Microservices = microservices;
        }
    }
}
