using BetterCoding.Strapi.SDK.Core.Webhook;

namespace BetterCoding.MessagePubSubCenter.Services.Pipelines.Webhook
{
    public class WebhookPayloadContext
    {
        public WebhookPayloadContext(WebhookPayload payload)
        {
            Payload = payload;
        }

        public WebhookPayload Payload { get; set; }

        public Dictionary<string,object> ServerFeteched { get; set; }
    }
}
