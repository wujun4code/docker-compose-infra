using BetterCoding.Strapi.SDK.Core.Webhook;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public interface IStrapiWebhookService
    {
        Task PublishMessageAsync(IWebhookPayload strapiWebhookPayload, CancellationToken stoppingToken = default);
        Task SyncToElasticSearch(IWebhookPayload webhookPayload);
    }
}
