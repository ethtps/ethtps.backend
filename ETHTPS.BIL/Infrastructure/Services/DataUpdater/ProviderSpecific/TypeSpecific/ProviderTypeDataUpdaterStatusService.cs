using ETHTPS.Data.Core.Models.DataUpdater;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.ProviderSpecific.TypeSpecific
{
    public sealed class ProviderTypeDataUpdaterStatusService : IProviderTypeDataUpdaterStatusService
    {
        private readonly IProviderDataUpdaterStatusManager _providerDataUpdaterStatusManager;
        private string _providerName => _providerDataUpdaterStatusManager.ProviderName;

        public ProviderTypeDataUpdaterStatusService(IProviderDataUpdaterStatusManager providerDataUpdaterStatusManager, UpdaterType type)
        {
            _providerDataUpdaterStatusManager = providerDataUpdaterStatusManager;
            UpdaterType = type;
        }

        public static ProviderTypeDataUpdaterStatusService From(IProviderDataUpdaterStatusManager providerDataUpdaterStatusManager, UpdaterType type) => new ProviderTypeDataUpdaterStatusService(providerDataUpdaterStatusManager, type);

        public UpdaterType UpdaterType { get; private set; }
        public string ProviderName => _providerDataUpdaterStatusManager.ProviderName;

        public bool? Enabled => _providerDataUpdaterStatusManager.GetStatusFor(_providerName, UpdaterType)?.Enabled;

        public IEnumerable<LiveDataUpdaterStatus> GetAllStatuses()
        {
            return _providerDataUpdaterStatusManager.GetAllStatuses();
        }

        public IEnumerable<LiveDataUpdaterStatus> GetStatusFor(string provider)
        {
            return _providerDataUpdaterStatusManager.GetStatusFor(provider);
        }

        public LiveDataUpdaterStatus? GetStatusFor(string provider, UpdaterType updaterType)
        {
            return _providerDataUpdaterStatusManager.GetStatusFor(provider, updaterType);
        }

        public void MarkAsFailed(string provider, UpdaterType updaterType)
        {
            _providerDataUpdaterStatusManager.MarkAsFailed(provider, updaterType);
        }

        public void MarkAsRanSuccessfully(string provider, UpdaterType updaterType)
        {
            _providerDataUpdaterStatusManager.MarkAsRanSuccessfully(provider, updaterType);
        }

        public void MarkAsRunning(string provider, UpdaterType updaterType)
        {
            _providerDataUpdaterStatusManager.MarkAsRunning(provider, updaterType);
        }

        public void SetStatusFor(string provider, UpdaterType updaterType, UpdaterStatus status)
        {
            _providerDataUpdaterStatusManager.SetStatusFor(provider, updaterType, status);
        }

        public void IncrementNumberOfSuccesses(string provider, UpdaterType updaterType)
        {
            _providerDataUpdaterStatusManager.IncrementNumberOfSuccesses(provider, updaterType);
        }

        public void IncrementNumberOfFailures(string provider, UpdaterType updaterType)
        {
            _providerDataUpdaterStatusManager.IncrementNumberOfFailures(provider, updaterType);
        }

        public DateTime? GetLastRunTime()
        {
            return _providerDataUpdaterStatusManager.GetLastRunTimeFor(UpdaterType);
        }

        public DateTime? GetLastRunTimeFor(UpdaterType updaterType)
        {
            return _providerDataUpdaterStatusManager.GetLastRunTimeFor(updaterType);
        }

        public DateTime? GetLastRunTimeFor(string provider, UpdaterType updaterType)
        {
            return _providerDataUpdaterStatusManager.GetLastRunTimeFor(provider, updaterType);
        }
    }
}
