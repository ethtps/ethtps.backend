using ETHTPS.Data.Core.Models.DataUpdater;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataUpdater
{
    public interface IDataUpdaterStatusGetter
    {
        IEnumerable<LiveDataUpdaterStatus?> GetAllStatuses();
        IEnumerable<LiveDataUpdaterStatus?> GetStatusFor(string provider);
        LiveDataUpdaterStatus? GetStatusFor(string provider, UpdaterType updaterType);
        DateTime? GetLastRunTimeFor(string provider, UpdaterType updaterType);
        TimeSpan? GetTimeSinceLastRanFor(string provider, UpdaterType updaterType) => DateTime.Now - GetLastRunTimeFor(provider, updaterType);
    }
}
