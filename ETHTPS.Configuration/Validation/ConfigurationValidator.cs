using ETHTPS.Configuration.Database;
using ETHTPS.Configuration.Validation.Exceptions;
using ETHTPS.Configuration.Validation.Models;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using static ETHTPS.Configuration.Constants;

namespace ETHTPS.Configuration.Validation
{
    /// <summary>
    /// Represents a validator that makes sure the database provides the necessary configuration strings.
    /// </summary>
    internal class ConfigurationValidator
    {
        private readonly ConfigurationContext _context;
        private StartupConfigurationModel? _startupConfiguration;
        private static bool _validated = false;
        private readonly ILogger<ConfigurationValidator>? _logger;

        public ConfigurationValidator(ConfigurationContext context, ILogger<ConfigurationValidator>? logger)
        {
            if (!File.Exists(STARTUP_CONFIG_FILENAME))
                throw new ConfigurationNotFoundException("No startup configuration found", STARTUP_CONFIG_FILENAME);

            _startupConfiguration = JsonConvert.DeserializeObject<StartupConfigurationModel>(File.ReadAllText(STARTUP_CONFIG_FILENAME));
            if (_startupConfiguration == null)
                throw new ConfigurationNotFoundException("Couldn't load configuration", STARTUP_CONFIG_FILENAME);

            _context = context;
            _logger = logger;
        }

        public void ThrowIfConfigurationInvalid()
        {
            if (!_validated)
            {
                ValidateMicroservices();
                ValidateRequiredConfigurationStrings();
                WarnOfMissingOptionalStrings();
                _validated = true;
            }
        }

        private void WarnOfMissingOptionalStrings()
        {
            throw new NotImplementedException();
        }

        private void ValidateRequiredConfigurationStrings()
        {
            throw new NotImplementedException();
        }

        private void ValidateMicroservices()
        {
            lock (_context.LockObj)
            {
                List<string>? existingMicroservices = _context.Microservices?.Select(x => x.Name).ToList();
                if (_startupConfiguration.Required.MicroserviceConfiguration.Select(x => existingMicroservices?.Contains(x.Name ?? string.Empty)).Any(x => x == false))
                {
                    throw new MicroservicesNotFoundException(_startupConfiguration.Required.MicroserviceConfiguration.Where(x => !existingMicroservices.Contains(x.Name)).Select(x => x.Name ?? string.Empty).ToArray());
                }
            }
        }
    }
}
