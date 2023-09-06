using System.Reflection;

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
    public sealed class ConfigurationValidator
    {
        private readonly ConfigurationContext _context;
        private readonly StartupConfigurationModel? _startupConfiguration;
        private static bool _validated = false;
        private readonly ILogger<ConfigurationValidator>? _logger;

        public ConfigurationValidator(ConfigurationContext context, ILogger<ConfigurationValidator>? logger)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, STARTUP_CONFIG_FILENAME);
            if (!File.Exists(path))
                throw new ConfigurationNotFoundException($"No startup configuration file found ({path})", STARTUP_CONFIG_FILENAME);

            _startupConfiguration = JsonConvert.DeserializeObject<StartupConfigurationModel>(File.ReadAllText(path));
            if (_startupConfiguration == null)
                throw new ConfigurationNotFoundException("Couldn't load configuration", path);

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
            lock (_context.LockObj)
            {
                if (_startupConfiguration == null || _startupConfiguration.Required == null || _startupConfiguration.Required.MicroserviceConfiguration == null)
                    return;
            }

            lock (_context.LockObj)
            {
                foreach (var microserviceConfiguration in _startupConfiguration.Required.MicroserviceConfiguration)
                {
                    if (microserviceConfiguration.RequiredConfigurationStrings == null) continue;
                    foreach (var requiredString in microserviceConfiguration.RequiredConfigurationStrings)
                    {
                        var s = _context.MicroserviceConfigurationStrings?.FirstOrDefault(x => (x.Microservice != null ? x.Microservice.Name : Microservice.EMPTY.Name) == microserviceConfiguration.Name && (x.ConfigurationString != null ? x.ConfigurationString.Name : ConfigurationString.EMPTY.Name) == requiredString);
                        if (s == null)
                        {
                            _logger?.LogWarning($"{requiredString} doesn't exist");
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace((s.ConfigurationString != null ? s.ConfigurationString.Value : ConfigurationString.EMPTY.Value)))
                            {
                                _logger?.LogWarning($"{requiredString} is null or empty");
                            }
                        }
                    }
                }
            }
        }

        private void ValidateRequiredConfigurationStrings()
        {
            lock (_context.LockObj)
            {
                if (_startupConfiguration == null || _startupConfiguration.Required == null || _startupConfiguration.Required.MicroserviceConfiguration == null)
                    return;
            }

            var missingConfigurationStrings = new List<string>();
            var invalidConfigurationStrings = new List<string>();

            lock (_context.LockObj)
            {
                foreach (var microserviceConfiguration in _startupConfiguration.Required.MicroserviceConfiguration)
                {
                    if (microserviceConfiguration.RequiredConfigurationStrings == null) continue;
                    foreach (var requiredString in microserviceConfiguration.RequiredConfigurationStrings)
                    {
                        var s = (_context.MicroserviceConfigurationStrings?.FirstOrDefault(x => (x.Microservice != null ? x.Microservice.Name : Microservice.EMPTY.Name) == microserviceConfiguration.Name && (x.ConfigurationString != null ? x.ConfigurationString.Name : ConfigurationString.EMPTY.Name) == requiredString));

                        if (s == null)
                        {
                            missingConfigurationStrings.Add($"{{{microserviceConfiguration.Name}.{requiredString}}}");
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(s.ConfigurationString?.Value))
                            invalidConfigurationStrings.Add($"{{{microserviceConfiguration.Name}.{s.ConfigurationString?.Name}}}");
                    }
                }
            }

            if (missingConfigurationStrings.Any())
                throw new Exception($"Missing the following configuration strings strings: [{string.Join(',', missingConfigurationStrings)}]", new ConfigurationNotFoundException());
            if (invalidConfigurationStrings.Any())
                throw new Exception($"Invalid configuration strings: [{string.Join(',', missingConfigurationStrings)}]", new ConfigurationNotFoundException());
        }


        private void ValidateMicroservices()
        {
            lock (_context.LockObj)
            {
                if (_startupConfiguration == null || _startupConfiguration.Required == null || _startupConfiguration.Required.MicroserviceConfiguration == null)
                    return;
            }

            lock (_context.LockObj)
            {
                List<string>? existingMicroservices = _context.Microservices?.Select(x => x.Name).ToList();
                if (_startupConfiguration.Required.MicroserviceConfiguration.Select(x => existingMicroservices?.Contains(x.Name ?? string.Empty)).Any(x => x == false))
                {
                    throw new MicroservicesNotFoundException(_startupConfiguration.Required.MicroserviceConfiguration.Where(x => !existingMicroservices?.Contains(x.Name ?? "undefined") ?? true).Select(x => x.Name ?? string.Empty).ToArray());
                }
            }
        }
    }
}
