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

        public bool IndexExist { get; set; }
        public Elastic.Clients.Elasticsearch.Id ElasticSearchId { get; set; }
    }
}
