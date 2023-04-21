using System.Runtime.Serialization;

namespace ETHTPS.Configuration.Validation.Exceptions
{
    public class ConfigurationNotFoundException : FileNotFoundException
    {
        public ConfigurationNotFoundException()
        {
        }

        public ConfigurationNotFoundException(string? message) : base(message)
        {
        }

        public ConfigurationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public ConfigurationNotFoundException(string? message, string? fileName) : base(message, fileName)
        {
        }

        public ConfigurationNotFoundException(string? message, string? fileName, Exception? innerException) : base(message, fileName, innerException)
        {
        }

        protected ConfigurationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
