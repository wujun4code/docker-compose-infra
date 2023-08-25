using BetterCoding.MessagePubSubCenter.Repository.ElasticSearch;
using BetterCoding.Strapi.SDK.Core.Services;
using BetterCoding.Strapi.SDK.Core.Webhook;
using Elastic.Clients.Elasticsearch;

namespace BetterCoding.MessagePubSubCenter.Services
{
    public class StrapiWebhookEventElasticSearchRepository : ElasticSearchRepository<IWebhookPayload>
    {
        private readonly IWebhookEventClassMapping _webhookEventClassMapping;
        public StrapiWebhookEventElasticSearchRepository(ElasticsearchClient client, IWebhookEventClassMapping webhookEventClassMapping) : base(client)
        {
            _webhookEventClassMapping = webhookEventClassMapping;
        }

        public override string GetClassName(IWebhookPayload entity)
        {
            return _webhookEventClassMapping.GetClassName(entity);
        }
    }
}
