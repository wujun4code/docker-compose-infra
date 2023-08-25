﻿using BetterCoding.Strapi.SDK.Core.Webhook;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public interface IStrapiWebhookService
    {
        Task PublishMessageAsync(WebhookPayload strapiWebhookPayload);
    }
}
