using ETHTPS.Data.Core.Models.DataUpdater;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.ProviderSpecific
{
    public interface IProviderDataUpdaterStatusGetter : IDataUpdaterStatusGetter
    {
        string ProviderName { get; }
        IEnumerable<LiveDataUpdaterStatus> GetStatus() => GetStatusFor(ProviderName);
        LiveDataUpdaterStatus? GetStatusFor(UpdaterType updaterType) => GetStatusFor(ProviderName, updaterType);
        DateTime? GetLastRunTimeFor(UpdaterType updaterType);
        TimeSpan? GetTimeSinceLastRanFor(UpdaterType updaterType) => DateTime.Now - GetLastRunTimeFor(ProviderName, updaterType);
    }
}
