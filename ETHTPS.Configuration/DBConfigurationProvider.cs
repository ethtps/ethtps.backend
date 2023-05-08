﻿using ETHTPS.Configuration.Database;
using ETHTPS.Configuration.Validation;
using ETHTPS.Data.Core.Extensions;

using Microsoft.Extensions.Logging;

namespace ETHTPS.Configuration
{
    public sealed class DBConfigurationProvider : IDBConfigurationProvider
    {
        private readonly ConfigurationContext _context;
        private readonly ILogger<ConfigurationValidator> _logger;
        private readonly string _environment;
        private readonly int _environmentID;

        public DBConfigurationProvider(ConfigurationContext context, ILogger<ConfigurationValidator>? logger, string environment = Constants
            .ENVIRONMENT)
        {
            _logger = logger;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            if (_context?.Environments == null)
                throw new ArgumentNullException(nameof(_context.Environments));

            _environment = environment;
            AddEnvironments(environment);
            lock (_context.LockObj)
            {
                _environmentID = _context.Environments.First(x => x.Name.ToUpper() == environment.ToUpper()).Id;
            }
            var validator = new ConfigurationValidator(context, logger);
            validator.ThrowIfConfigurationInvalid();
        }

        IDBConfigurationProvider IDBConfigurationProvider.this[string environment] { get => new DBConfigurationProvider(this._context, _logger, environment); }

        public void AddEnvironments(params string[] environments)
        {
            lock (_context.LockObj)
            {
                try
                {
                    var existing = _context.Environments?.Select(x => x.Name).ToList();
                    var toAdd = environments?.Where(x => !existing.Contains(x));
                    _context.Environments?.AddRange(toAdd.SafeSelect(x => new Database.Environment()
                    {
                        Name = x
                    }));
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"Error adding environments {string.Join(", ", environments)}");
                }
            }
        }

        public void AddMicroservice(string name, string? description)
        {
            lock (_context.LockObj)
            {
                if (!_context.Microservices.Any(x => x.Name.ToUpper() == name.ToUpper()))
                {
                    _context.Add
                        (new Microservice()
                        {
                            Name = name,
                            Description = description
                        });
                    _context.SaveChanges();
                }
            }
        }

        public void Dispose()
        {

        }

        public IEnumerable<IConfigurationString> GetConfigurationStrings(string name)
        {
            lock (_context.LockObj)
            {
                return _context.ConfigurationStrings.Where(x => x.Name.ToUpper() == name.ToUpper()).ToList();
            }
        }

        public IEnumerable<IConfigurationString> GetConfigurationStringsForMicroservice(IMicroservice microservice) => GetConfigurationStringsForMicroservice(microservice.Name);

        public IEnumerable<IConfigurationString> GetConfigurationStringsForMicroservice(string microserviceName)
        {
            lock (_context.LockObj)
            {
                return _context.MicroserviceConfigurationStrings
                    .Where(x => x.Microservice.Name.ToUpper() == microserviceName.ToUpper() && x.Environment.Name.ToUpper() == _environment.ToUpper() || x.Environment.Name.ToUpper() == "ALL")
                    .Select(x => (IConfigurationString)x.ConfigurationString)
                    .WhereNotNull()
                    .Select(x => new ConfigurationString()
                    {
                        Name = x.Name,
                        Value = x.Value
                    })
                    .ToList();
            }
        }

        public IEnumerable<IConfigurationString> GetConfigurationStringsForProvider(string provider)
        {
            lock (_context.LockObj)
            {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return _context.ProviderConfigurationStrings?
                    .Where(x => x.Provider.Name.ToUpper() == provider.ToUpper() && x.Environment.Name.ToUpper() == _environment.ToUpper() || x.Environment.Name.ToUpper() == "ALL")
                    .WhereNotNull()
                    .Select(x => (IConfigurationString?)x.ConfigurationString)
                    .ToList() ?? Enumerable.Empty<IConfigurationString>();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            }
        }

        public int GetEnvironmentID(string name)
        {
            lock (_context.LockObj)
            {
                return _context.Environments.First(x => x.Name.ToUpper() == name.ToUpper()).Id
                    ;
            }
        }

        public IEnumerable<string> GetEnvironments()
        {
            lock (_context.LockObj)
            {
                return _context.Environments.Select(x => x.Name).ToList();
            }
        }

        public int GetMicroserviceID(string name, bool addIfItDoesntExist = false)
        {
            lock (_context.LockObj)
            {
                Func<Microservice, bool> selector = x => x.Name.ToUpper() == name.ToUpper();
                if (!_context.Microservices.Any(selector))
                {
                    if (addIfItDoesntExist)
                    {
                        AddMicroservice(name, string.Empty);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"Microservice {name} doesn't exist and {nameof(addIfItDoesntExist)} is false");
                    }
                }

                return _context.Microservices.First(selector).Id
                    ;
            }
        }

        public IEnumerable<IMicroservice> GetMicroservices()
        {
            lock (_context.LockObj)
            {
                return _context.Microservices.ToList();
            }
        }

        public int SetConfigurationString(IConfigurationString configString)
        {
            lock (_context.LockObj)
            {
                if (!_context.ConfigurationStrings.Any(x => x.Name == configString.Name && x.Value == configString.Value))
                {
                    _context.ConfigurationStrings.Add(new ConfigurationString()
                    {
                        Name = configString.Name,
                        Value = configString.Value
                    });
                    _context.SaveChanges();
                }

                return _context.ConfigurationStrings.First(x => x.Name == configString.Name && x.Value == configString.Value).Id;
            }
        }

        public void SetConfigurationStringForMicroservice(IMicroservice microservice, IConfigurationString configString) => SetConfigurationStringForMicroservice(microservice.Name, configString);

        public void SetConfigurationStringForMicroservice(string microserviceName, IConfigurationString configString)
        {
            lock (_context.LockObj)
            {
                var microserviceID = _context.Microservices.First(x => x.Name.ToUpper().Equals(microserviceName.ToUpper())).Id;
                var environmentID = _context.Environments.First(x => x.Name.ToUpper().Equals(_environment.ToUpper())).Id;
                _context.MicroserviceConfigurationStrings.Add(new MicroserviceConfigurationString()
                {
                    ConfigurationString = new ConfigurationString()
                    {
                        Name = configString.Name,
                        Value = configString.Value
                    },
                    EnvironmentId = environmentID,
                    MicroserviceId = microserviceID
                });
                _context.SaveChanges();
            }
        }

        public void SetConfigurationStringsForProvider(string provider, params IConfigurationString[] configStrings)
        {
            lock (_context.LockObj)
            {
                var providerID = _context.Providers.First(x => x.Name.ToUpper().Equals(provider.ToUpper())).Id;
                var environmentID = _context.Environments.First(x => x.Name.ToUpper().Equals(_environment.ToUpper())).Id;
                configStrings.ToList().ForEach(configString => _context.ProviderConfigurationStrings.Add(new ProviderConfigurationString()
                {
                    ConfigurationString = new ConfigurationString()
                    {
                        Name = configString.Name,
                        Value = configString.Value
                    },
                    EnvironmentId = environmentID,
                    ProviderId = providerID
                }));
                _context.SaveChanges();
            }
        }
    }
}
