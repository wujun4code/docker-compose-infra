namespace BetterCoding.MessagePubSubCenter.Services.Pipelines.Webhook
{
    public class Entry : Dictionary<string, object>
    {
        public Entry(WebhookPayloadContext context) : base(context.Payload.Entry)
        {
            Context = context;
        }

        public WebhookPayloadContext Context { get; set; }
        public string CollectionName => Context.Payload.Model;
    }
}
