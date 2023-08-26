namespace BetterCoding.MessagePubSubCenter.Services.Pipelines.Webhook
{
    public class EntryLog
    {
        public EntryLog(WebhookPayloadContext context)
        {
            Context = context;
        }

        public WebhookPayloadContext Context { get; set; }
        public string CollectionName => $"{Context.Payload.Model}-audit-log";
    }
}
