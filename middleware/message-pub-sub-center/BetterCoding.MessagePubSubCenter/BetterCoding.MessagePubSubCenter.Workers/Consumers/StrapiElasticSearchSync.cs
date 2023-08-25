using BetterCoding.MessagePubSubCenter.Repository.ElasticSearch;
using BetterCoding.MessagePubSubCenter.Services;
using BetterCoding.Strapi.SDK.Core.Webhook;
using Newtonsoft.Json;

namespace BetterCoding.MessagePubSubCenter.Workers.Consumers
{
    public class StrapiElasticSearchSync : BackgroundService
    {
        private readonly ILogger<StrapiElasticSearchSync> _logger;
        private readonly IStrapiWebhookService _strapiWebhookService;
        private readonly IElasticSearchRepository<IWebhookPayload> _elasticSearchRepository;
        public StrapiElasticSearchSync(
            ILogger<StrapiElasticSearchSync> logger,
            IStrapiWebhookService strapiWebhookService,
            IElasticSearchRepository<IWebhookPayload> elasticSearchRepository)
        {
            _logger = logger;
            _strapiWebhookService = strapiWebhookService;
            _elasticSearchRepository = elasticSearchRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _strapiWebhookService.SubscribeAsync("sync-elasticsearch", Handle);
        }

        private async Task Handle(WebhookPayload message, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(message);
            await _elasticSearchRepository.AddAsync(message);
            _logger.LogInformation($"sync strapi data to elasticsearch: {json}");
        }
    }
}
