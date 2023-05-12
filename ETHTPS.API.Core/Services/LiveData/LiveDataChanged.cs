using Coravel.Events.Interfaces;

using ETHTPS.Data.Core.Models.LiveData.Triggers;

namespace ETHTPS.API.Core.Services.LiveData
{
    public class LiveDataChanged : IEvent
    {
        public List<L2DataUpdateModel> Updates { get; private set; } = new List<L2DataUpdateModel>();

        public LiveDataChanged(List<L2DataUpdateModel> updates)
        {
            Updates = updates;
        }
    }
}
