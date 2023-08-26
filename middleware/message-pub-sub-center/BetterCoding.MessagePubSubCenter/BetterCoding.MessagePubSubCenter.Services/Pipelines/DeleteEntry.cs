using BetterCoding.MessagePubSubCenter.Repository.ElasticSearch;
using BetterCoding.MessagePubSubCenter.Services.Pipelines.Webhook;
using BetterCoding.Patterns.Pipeline;

namespace BetterCoding.MessagePubSubCenter.Services.Pipelines
{
    public class DeleteEntry : AutomicTransactionPipeline<WebhookPayloadContext>
    {
        IElasticSearchRepository _elasticSearchRepository;
        public DeleteEntry(IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public override async Task<WebhookPayloadContext> ProcessAsync(WebhookPayloadContext input)
        {
            if (input.ServerFeteched == null) throw new MissingFieldException();

            var exist = input.ServerFeteched.TryGetValue("id", out var id);
            if(!exist) throw new MissingFieldException();

            if (id == null) throw new MissingFieldException(nameof(id));

            var validInt = int.TryParse(id.ToString(), out var idFromInt);
            if(!validInt) throw new InvalidDataException();

            Elastic.Clients.Elasticsearch.Id convertedId = idFromInt;

            await _elasticSearchRepository.DeleteAsync<Dictionary<string, object>, Elastic.Clients.Elasticsearch.Id>(convertedId, input.Payload.Model);
            return input;
        }

        public override Task<WebhookPayloadContext> RevertAsync(WebhookPayloadContext input)
        {
            return Task.FromResult(input);
        }
    }
}
