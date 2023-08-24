using BetterCoding.Strapi.SDK.Core;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public interface IStrapiWebhookService
    {
        Task PublishMessageAsync(StrapiWebhookPayload strapiWebhookPayload);
    }
}
