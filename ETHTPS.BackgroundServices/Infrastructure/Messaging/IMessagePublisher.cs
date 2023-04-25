namespace ETHTPS.Services.Infrastructure.Messaging
{
    public interface IMessagePublisher
    {
        void PublishMessage(string message, string queue, string host);
        void PublishJSONMessage<T>(T message, string queue, string host);
    }
}
