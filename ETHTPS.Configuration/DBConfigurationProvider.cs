using System.Data;

using ETHTPS.Configuration.Database;
using ETHTPS.Configuration.Validation;
using ETHTPS.Data.Core.Extensions;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ETHTPS.Configuration
{
    public sealed class DBConfigurationProvider : IDBConfigurationProvider
    {
        private readonly ConfigurationContext _context;
        private readonly ILogger<ConfigurationValidator>? _logger;
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

        IDBConfigurationProvider IDBConfigurationProvider.this[string environment]
        {
            get
            {
                lock (_context.LockObj)
                {
                    return new DBConfigurationProvider(this._context, _logger, environment);
                }
            }
        }

        public void AddEnvironments(params string[] environments)
        {
            lock (_context.LockObj)
            {
                try
                {
                    var existing = _context.Environments?.Select(x => x.Name).ToList() ?? Enumerable.Empty<string>();
                    var toAdd = environments?.Where(x => !existing.Contains(x));
                    _context.Environments?.AddRange(toAdd.SafeSelect(x => new Database.Environment()
                    {
                        Name = x
                    }));
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex,
                        $"Error adding environments {string.Join(", ", environments ?? Array.Empty<string>())}");
                }
            }
        }

        public void AddMicroservice(string name, string? description)
        {
            lock (_context.LockObj)
            {
                if (!_context.Microservices?.Any(x => x.Name.ToUpper() == name.ToUpper()) ?? true)
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

        public IEnumerable<AllConfigurationStringsModel> GetAllConfigurationStrings()
        {
            lock (_context.LockObj)
            {
                return _context.Database.SqlQueryRaw<AllConfigurationStringsModel>("EXEC [Configuration].[GetAllConfigurationStrings]");
            }
        }

        public ConfigurationStringLinksModel GetAllLinks(int configurationStringId)
        {
            lock (_context.LockObj)
            {
                var allLinksModels = _context.Database.SqlQueryRaw<AllLinksModel?>(
                    "EXEC [Configuration].[GetAllLinksForConfigurationString] @ConfigurationStringID",
                    new SqlParameter("ConfigurationStringID", configurationStringId)).ToList();

                var first = allLinksModels.First(x => x != null) ?? throw new Exception("Something's up");
                ConfigurationStringLinksModel result = new()
                {
                    ConfigurationString = new ConfigurationString
                    {
                        Id = first.ConfigurationStringID,
                        Name = first.ConfigurationStringName,
                        Value = first.ConfigurationStringValue
                    },
                    ProviderLinks = allLinksModels
                        .Where(model => model?.ProviderLinksID.HasValue ?? false)
                        .Select(model => model == null ? null : new ProviderConfigurationString
                        {
                            Id = model.ProviderLinksID ?? -1,
                            ProviderId = model.ProviderLinksProviderID ?? -1,
                            ConfigurationStringId = model.ProviderLinksConfigurationStringID ?? -1,
                            EnvironmentId = model.ProviderLinksEnvironmentID ?? -1
                        }).ToList(),
                    MicroserviceLinks = allLinksModels
                        .Where(model => model?.MicroserviceLinksConfigurationStringID.HasValue ?? false)
                        .Select(model => model == null ? null : new MicroserviceConfigurationString
                        {
                            Id = model.MicroserviceLinksID ?? -1,
                            MicroserviceId = model.MicroserviceLinksMicroserviceID ?? -1,
                            ConfigurationStringId = model.MicroserviceLinksConfigurationStringID ?? -1,
                            EnvironmentId = model.MicroserviceLinksEnvironmentID ?? -1
                        }).ToList()
                };

                return result;
            }
        }



        public int AddOrUpdateConfigurationString(ConfigurationStringUpdateModel configurationString, string? microservice, string? environment)
        {
            lock (_context.LockObj)
            {
                var configStringIdParam = new SqlParameter
                {
                    ParameterName = "@ConfigStringID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw(
                   "EXECUTE [Configuration].[InsertOrUpdateConfigurationString] @MicroserviceName, @EnvironmentName, @ConfigStringName, @ConfigStringValue, @IsSecret, @IsEncrypted, @ConfigStringID OUT",
                   new SqlParameter("@MicroserviceName", microservice ?? (object)DBNull.Value),
                   new SqlParameter("@EnvironmentName", environment ?? (object)DBNull.Value),
                   new SqlParameter("@ConfigStringName", configurationString.Name),
                   new SqlParameter("@ConfigStringValue", configurationString.Value),
                   new SqlParameter("@IsSecret", configurationString.IsSecret),
                   new SqlParameter("@IsEncrypted", configurationString.IsEncrypted),
                   configStringIdParam);
                _context.SaveChanges();
                if (configStringIdParam.Value != DBNull.Value)
                {
                    return (int)configStringIdParam.Value;
                }

                return -2;
            }
        }

        public int LinkProviderToConfigurationString(string providerName, string configurationStringName,
            string environmentName = Constants.ENVIRONMENT)
        {
            // Create parameters
            var providerNameParam = new SqlParameter("@ProviderName", providerName);
            var configurationStringNameParam = new SqlParameter("@ConfigurationStringName", configurationStringName);
            var environmentNameParam = new SqlParameter("@EnvironmentName", environmentName);
            var returnCodeParam = new SqlParameter("@RC", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            lock (_context.LockObj)
            {
                _context.Database.ExecuteSqlRaw(
                    "EXEC @RC = [Configuration].[LinkProviderToConfigurationString] @ProviderName, @ConfigurationStringName, @EnvironmentName",
                    returnCodeParam, providerNameParam, configurationStringNameParam, environmentNameParam);
            }

            return (int)returnCodeParam.Value;
        }

        public int LinkProviderToConfigurationString(int providerID, int configurationStringID,
            string environmentName = Constants.ENVIRONMENT)
        {
            var providerIdParam = new SqlParameter("@ProviderID", providerID);
            var configurationStringIdParam = new SqlParameter("@ConfigurationStringID", configurationStringID);
            var environmentNameParam = new SqlParameter("@EnvironmentName", environmentName);
            var result = 0;
            lock (_context.LockObj)
            {
                result = _context.Database.ExecuteSqlRaw("[Configuration].[LinkProviderToConfigurationStringByID] @ProviderID, @ConfigurationStringID, @EnvironmentName",
                    providerIdParam, configurationStringIdParam, environmentNameParam);
            }

            return result;
        }

        public int UnlinkProviderFromConfigurationString(int providerID, int configurationStringID,
            string environmentName = Constants.ENVIRONMENT)
        {
            var providerIdParam = new SqlParameter("@ProviderID", providerID);
            var configurationStringIdParam = new SqlParameter("@ConfigurationStringID", configurationStringID);
            var environmentNameParam = new SqlParameter("@EnvironmentName", environmentName);
            var result = 0;
            lock (_context.LockObj)
            {
                result = _context.Database.ExecuteSqlRaw("[Configuration].[UnlinkProviderFromConfigurationStringByID] @ProviderID, @ConfigurationStringID, @EnvironmentName",
                    providerIdParam, configurationStringIdParam, environmentNameParam);
            }

            return result;
        }

        public int ClearHangfireQueue()
        {
            lock (_context.LockObj)
            {
                return _context.Database.ExecuteSqlRaw("EXEC [Hangfire].[DeleteAllJobs]");
            }
        }


        public IEnumerable<IConfigurationString>? GetConfigurationStrings(string name)
        {
            lock (_context.LockObj)
            {
                return _context.ConfigurationStrings?.Where(x => x.Name.ToUpper() == name.ToUpper()).ToList();
            }
        }

        public IEnumerable<IConfigurationString>? GetConfigurationStringsForMicroservice(IMicroservice microservice) => GetConfigurationStringsForMicroservice(microservice.Name);

        public IEnumerable<IConfigurationString>? GetConfigurationStringsForMicroservice(string microserviceName)
        {
            lock (_context.LockObj)
            {
                var emptyEnvironment = Database.Environment.EMPTY;
                var emptyMicroservice = Database.Microservice.EMPTY;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return (_context.MicroserviceConfigurationStrings?
                        .Where(x => (x.Microservice != null ? x.Microservice.Name : emptyMicroservice.Name).ToUpper() == microserviceName.ToUpper()
                                    && (x.Environment != null ? x.Environment.Name : emptyEnvironment.Name).ToUpper() == _environment.ToUpper()
                                    || (x.Environment != null ? x.Environment.Name : emptyEnvironment.Name).ToUpper() == "ALL")
                        .Select(x => (IConfigurationString?)x.ConfigurationString))?
                    .AsEnumerable()
                    .WhereNotNull()
                    .Select(x => new ConfigurationString()
                    {
                        Name = x.Name,
                        Value = x.Value
                    })
                    .ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }

        public IEnumerable<IConfigurationString> GetConfigurationStringsForProvider(string provider) => _context.GetConfigurationStrings(provider, _environment);

        public int? GetEnvironmentID(string name)
        {
            lock (_context.LockObj)
            {
                return _context.Environments?.First(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase)).Id;
            }
        }

        public IEnumerable<string>? GetEnvironments()
        {
            lock (_context.LockObj)
            {
                return _context.Environments?.Select(x => x.Name).ToList();
            }
        }

        public int? GetMicroserviceID(string name, bool addIfItDoesntExist = false)
        {
            lock (_context.LockObj)
            {
                Func<Microservice, bool> selector = x => x.Name.ToUpper() == name.ToUpper();
                if (!_context.Microservices?.Any(selector) ?? false)
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

                return _context.Microservices?.First(selector).Id;
            }
        }

        public IEnumerable<IMicroservice>? GetMicroservices()
        {
            lock (_context.LockObj)
            {
                return _context.Microservices?.ToList();
            }
        }

        public int? SetConfigurationString(IConfigurationString configString)
        {
            lock (_context.LockObj)
            {
                if (!_context.ConfigurationStrings?.Any(x => x.Name == configString.Name && x.Value == configString.Value) ?? true)
                {
                    _context.ConfigurationStrings?.Add(new ConfigurationString()
                    {
                        Name = configString.Name,
                        Value = configString.Value
                    });
                    _context.SaveChanges();
                }

                return _context.ConfigurationStrings?.First(x => x.Name == configString.Name && x.Value == configString.Value).Id;
            }
        }

        public void SetConfigurationStringForMicroservice(IMicroservice microservice, IConfigurationString configString) => SetConfigurationStringForMicroservice(microservice.Name, configString);

        public void SetConfigurationStringForMicroservice(string microserviceName, IConfigurationString configString)
        {
            lock (_context.LockObj)
            {
                var microserviceID = (_context.Microservices?.First(x => x.Name.ToUpper().Equals(microserviceName.ToUpper())).Id) ?? throw new ArgumentException($"{nameof(_context.Microservices)} is null");
                var environmentID = (_context.Environments?.First(x => x.Name.ToUpper().Equals(_environment.ToUpper())).Id) ?? throw new ArgumentException($"{nameof(_context.Environments)} is null");
                _context.MicroserviceConfigurationStrings?.Add(new MicroserviceConfigurationString()
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
                var providerID = _context.Providers?.First(x => x.Name.ToUpper().Equals(provider.ToUpper())).Id ?? throw new ArgumentException($"{nameof(_context.Providers)} is null");
                var environmentID = _context.Environments?.First(x => x.Name.ToUpper().Equals(_environment.ToUpper())).Id ?? throw new ArgumentException($"{nameof(_context.Environments)} is null");
                configStrings.ToList().ForEach(configString => _context.ProviderConfigurationStrings?.Add(new ProviderConfigurationString()
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
