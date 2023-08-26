using Elastic.Clients.Elasticsearch;

namespace BetterCoding.MessagePubSubCenter.Repository.ElasticSearch
{
    public interface IElasticSearchRepository<T> : IRepository<T>
    {
        ElasticsearchClient Client { get; }
        IRepositoryHub RepositoryHub { get; }
    }

    public interface IElasticSearchRepository : IRepository
    {
        ElasticsearchClient Client { get; }
        IRepositoryHub RepositoryHub { get; }
    }

    public abstract class ElasticSearchRepository : IElasticSearchRepository
    {
        protected readonly ElasticsearchClient _client;
        public ElasticsearchClient Client => _client;

        protected readonly IRepositoryHub _repositoryHub;
        public IRepositoryHub RepositoryHub => _repositoryHub;

        public ElasticSearchRepository(
            ElasticsearchClient client,
            IRepositoryHub repositoryHub)
        {
            _client = client;
            _repositoryHub = repositoryHub;
        }

        public virtual async Task<T> AddAsync<T>(T entity, string collectionName = "")
        {
            var indexName = string.IsNullOrEmpty(collectionName) ? _repositoryHub.CollectionNameMapping.GetCollectionName(entity) : collectionName;
            var response = await _client.IndexAsync(entity, indexName);
            if (!response.IsValidResponse)
            {
                if (response.ApiCallDetails.OriginalException != null)
                    throw response.ApiCallDetails.OriginalException;
                else throw new InvalidOperationException(response.ApiCallDetails.ToString());
            }

            return entity;
        }
    }

    public class EasyElasticSearchRepository : ElasticSearchRepository
    {
        public EasyElasticSearchRepository(ElasticsearchClient client, IRepositoryHub repositoryHub)
            : base(client, repositoryHub)
        {

        }
    }

    public abstract class ElasticSearchRepository<T> : IElasticSearchRepository<T>
    {
        public ElasticsearchClient Client => _elasticSearchRepository.Client;

        protected readonly IElasticSearchRepository _elasticSearchRepository;

        public IRepositoryHub RepositoryHub => _elasticSearchRepository.RepositoryHub;

        public ElasticSearchRepository(
           IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public abstract string GetClassName(T entity);

        public virtual async Task<T> AddAsync(T entity)
        {
            await _elasticSearchRepository.AddAsync(entity, GetClassName(entity));
            return entity;
        }
    }
}
