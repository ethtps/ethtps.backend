namespace ETHTPS.Services.BlockchainServices.Status.BackgroundTasks.Discord
{
    public sealed class WebhookMessage
    {
        public string content { get; set; }
        public Embed[] embeds { get; set; }
    }
}
