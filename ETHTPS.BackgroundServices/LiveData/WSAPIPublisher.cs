using ETHTPS.Data.Core.Models.LiveData.Triggers;
using ETHTPS.Services.Infrastructure.Messaging;

namespace ETHTPS.Services.LiveData
{
    public class WSAPIPublisher
    {
        private readonly IMessagePublisher _messagePublisher;

        public WSAPIPublisher(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public void Push(L2DataUpdateModel model) => _messagePublisher.PublishJSONMessage(model, MessagingQueues.LIVEDATA_NEWDATAPOINT_QUEUE);
    }
}
