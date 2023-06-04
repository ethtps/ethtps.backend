using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater;
using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.ProviderSpecific;
using ETHTPS.Data.Core.Extensions;
using ETHTPS.Data.Core.Models.DataUpdater;
using ETHTPS.Data.Integrations.MSSQL;
using ETHTPS.Data.Integrations.MSSQL.Extensions;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.Updater
{
    public sealed class DataUpdaterStatusService : IDataUpdaterStatusService
    {
        private readonly EthtpsContext _context;

        public bool? Enabled { get; }

        public DataUpdaterStatusService(EthtpsContext context)
        {
            _context = context;
        }

        public IEnumerable<ETHTPS.Data.Core.Models.DataUpdater.LiveDataUpdaterStatus> GetAllStatuses()
        {
            lock (_context.LockObj)
            {
                return _context.LiveDataUpdaterStatuses.SafeSelect(x => Convert(x));
            }
        }

        public ETHTPS.Data.Core.Models.DataUpdater.LiveDataUpdaterStatus? GetStatusFor(string provider, UpdaterType updaterType)
        {
            lock (_context.LockObj)
            {
                var providerNameParam = new SqlParameter("@ProviderName", provider);
                var updaterTypeParam = new SqlParameter("@UpdaterType", updaterType.ToString());
                var temp = _context.Set<ETHTPS.Data.Integrations.MSSQL.LiveDataUpdaterStatus>()
                    .FromSqlRaw("EXEC [DataUpdaters].[GetLiveDataUpdaterStatus] @ProviderName, @UpdaterType", providerNameParam, updaterTypeParam)
                    .AsEnumerable().FirstOrDefault();
                if (temp == null) return null;
                return new ETHTPS.Data.Core.Models.DataUpdater.LiveDataUpdaterStatus()
                {
                    Enabled = temp?.Enabled,
                    LastSuccessfulRunTime = temp?.LastRunTime,
                    NumberOfFailures = temp?.NumberOfFailures ?? 0,
                    NumberOfSuccesses = temp?.NumberOfSuccesses ?? 0,
                    UpdaterType = temp?.Updater?.Type?.TypeName,
                    Status = temp?.Status?.Name,
                };
            }
        }

        public IEnumerable<ETHTPS.Data.Core.Models.DataUpdater.LiveDataUpdaterStatus?> GetStatusFor(string provider) => new[]
        {
            GetStatusFor(provider, UpdaterType.TPSGPS)
        };

        public void IncrementNumberOfFailures(string provider, UpdaterType updaterType)
        {
            var updater = GetUpdater(provider, updaterType);
            lock (_context.LockObj)
            {
                var x = _context.LiveDataUpdaterStatuses.First(x => x.UpdaterId == updater.Id);
                x.NumberOfFailures++;
                _context.LiveDataUpdaterStatuses.Update(x);
                _context.SaveChanges();
            }
        }

        public void IncrementNumberOfSuccesses(string provider, UpdaterType updaterType)
        {
            var updater = GetUpdater(provider, updaterType);
            lock (_context.LockObj)
            {
                var x = _context.LiveDataUpdaterStatuses.First(x => x.UpdaterId == updater.Id);
                x.NumberOfSuccesses++;
                x.LastSuccessfulRunTime = DateTime.Now;
                _context.LiveDataUpdaterStatuses.Update(x);
                _context.SaveChanges();
            }
        }

        public void MarkAsRanSuccessfully(string provider, UpdaterType updaterType)
        {
            SetStatusFor(provider, updaterType, UpdaterStatus.RanSuccessfully);
        }

        public void MarkAsFailed(string provider, UpdaterType updaterType)
        {
            SetStatusFor(provider, updaterType, UpdaterStatus.Failed);
        }

        public void SetStatusFor(string provider, UpdaterType updaterType, UpdaterStatus status)
        {
            var providerNameParam = new SqlParameter("@ProviderName", provider);
            var updaterTypeParam = new SqlParameter("@UpdaterType", updaterType.ToString());
            var updaterStatusParam = new SqlParameter("@UpdaterStatus", status.ToString());

            _context.Database.ExecuteSqlRaw("EXEC DataUpdaters.SetStatusFor @ProviderName, @UpdaterType, @UpdaterStatus", providerNameParam, updaterTypeParam, updaterStatusParam);
        }

        private DataUpdater GetUpdater(string provider, UpdaterType updaterType)
        {
            CreateUpdaterIfNecessary(provider, updaterType);
            lock (_context.LockObj)
            {
                return _context.DataUpdaters.First(x => x.Type.TypeName == updaterType.ToString() && x.Provider.Name == provider);
            }
        }
        private DataUpdaterStatus GetStatus(UpdaterStatus status) => GetStatus(status.ToString());
        private DataUpdaterStatus GetStatus(string status)
        {
            CreateStatusIfNecessary(status);
            lock (_context.LockObj)
            {
                return _context.DataUpdaterStatuses.First(x => x.Name == status);
            }
        }

        private void CreateStatusIfNecessary(string status)
        {
            lock (_context.LockObj)
            {
                if (!_context.DataUpdaterStatuses.Any(x => x.Name == status))
                {
                    _context.DataUpdaterStatuses.Add(new DataUpdaterStatus()
                    {
                        Name = status,
                    });
                    _context.SaveChanges();
                }
            }
        }

        private void CreateUpdaterIfNecessary(string provider, UpdaterType updaterType)
        {
            lock (_context.LockObj)
            {
                if (!_context.DataUpdaterTypes.Any(x => x.TypeName == updaterType.ToString()))
                {
                    _context.DataUpdaterTypes.Add(new DataUpdaterType()
                    {
                        TypeName = updaterType.ToString(),
                    });
                    _context.SaveChanges();
                }

                if (!_context.DataUpdaters.Any(x => x.Provider.Name == provider && x.Type.TypeName == updaterType.ToString()))
                {
                    var typeID = _context.DataUpdaterTypes.First(x => x.TypeName == updaterType.ToString()).Id;
                    var providerID = _context.GetProviderID(provider);
                    _context.DataUpdaters.Add(new DataUpdater()
                    {
                        ProviderId = providerID,
                        TypeId = typeID
                    });
                    _context.SaveChanges();
                }
            }
        }

        private static ETHTPS.Data.Core.Models.DataUpdater.LiveDataUpdaterStatus Convert(ETHTPS.Data.Integrations.MSSQL.LiveDataUpdaterStatus result) => new()
        {
            LastSuccessfulRunTime = result.LastSuccessfulRunTime,
            NumberOfFailures = result.NumberOfFailures,
            NumberOfSuccesses = result.NumberOfSuccesses,
            Status = result.Status?.Name,
            Updater = result.Updater?.Provider?.Name,
            UpdaterType = result.Updater?.Type?.TypeName,
            Enabled = result.Updater?.Enabled
        };

        public void MarkAsRunning(string provider, UpdaterType updaterType) => SetStatusFor(provider, updaterType, UpdaterStatus.Running);

        public IProviderDataUpdaterStatusService MakeProviderSpecific(string provider) => ProviderDataUpdaterStatusService.From(this, provider);

        public DateTime? GetLastRunTimeFor(string provider, UpdaterType updaterType)
        {
            var info = GetStatusFor(provider, updaterType);
            return info?.LastSuccessfulRunTime;
        }

        public async Task<IEnumerable<DataUpdaterDTO>> GetAllAsync() => await _context.Set<DataUpdaterDTO>().FromSqlRaw("EXECUTE [DataUpdaters].[GetAllDataUpdaters]")
         .ToListAsync();
    }
}
