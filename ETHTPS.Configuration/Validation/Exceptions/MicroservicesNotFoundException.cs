namespace ETHTPS.Configuration.Validation.Exceptions
{
    public sealed class MicroservicesNotFoundException : Exception
    {
        public string[] Microservices { get; private set; }

        public MicroservicesNotFoundException(string[] microservices)
        {
            Microservices = microservices;
        }

        public override string Message => $"The following microservices were not found: {string.Join(", ", Microservices)}";
    }
}
