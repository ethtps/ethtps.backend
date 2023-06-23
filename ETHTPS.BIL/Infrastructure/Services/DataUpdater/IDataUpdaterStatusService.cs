using ETHTPS.API.BIL.Infrastructure.Services.DataUpdater.ProviderSpecific;
using ETHTPS.Data.Core.Models.DataUpdater;

namespace ETHTPS.API.BIL.Infrastructure.Services.DataUpdater
{
    public interface IDataUpdaterStatusService : IDataUpdaterManager
    {
        IProviderDataUpdaterStatusService MakeProviderSpecific(string provider);
        Task<IEnumerable<DataUpdaterDTO>> GetAllAsync();
    }

}
