using BetterCoding.MessagePubSubCenter.Services;
using BetterCoding.Strapi.SDK.Core.Webhook;
using EasyNetQ;
using Newtonsoft.Json;

namespace BetterCoding.MessagePubSubCenter.Workers.Consumers
{
    public class StrapiElasticSearchSync : BackgroundService
    {
        private readonly ILogger<StrapiElasticSearchSync> _logger;
        private readonly IStrapiWebhookService _strapiWebhookService;

        public StrapiElasticSearchSync(
            ILogger<StrapiElasticSearchSync> logger,
            IStrapiWebhookService strapiWebhookService)
        {
            _logger = logger;
            _strapiWebhookService = strapiWebhookService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _strapiWebhookService.SubscribeAsync("sync-elasticsearch", Handle);
        }

        private async Task Handle(WebhookPayload message, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(message);
            _logger.LogInformation($"sync strapi data to elasticsearch: {json}");
        }
    }
}
