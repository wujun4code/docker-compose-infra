using BetterCoding.Strapi.SDK.Core.Webhook;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public interface IStrapiWebhookService
    {
        Task PublishMessageAsync(WebhookPayload strapiWebhookPayload);
        Task SubscribeAsync(string subscriptionId, Func<WebhookPayload, CancellationToken, Task> handler);
        Task SyncToElasticSearch(WebhookPayload webhookPayload);
    }
}
