using BetterCoding.Strapi.SDK.Core.Webhook;
using EasyNetQ;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public class StrapiWebhookService : IStrapiWebhookService
    {
        private readonly IBus _bus;

        public StrapiWebhookService(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishMessageAsync(WebhookPayload strapiWebhookPayload)
        {
            await _bus.PubSub.PublishAsync(strapiWebhookPayload, "strapi.webhook");
        }
    }
}
