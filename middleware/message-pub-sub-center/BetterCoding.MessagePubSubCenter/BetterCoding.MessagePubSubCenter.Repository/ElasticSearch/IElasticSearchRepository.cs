﻿using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;

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
        Task<T> AddAsync<T>(T entity, string collectionName = "");
        Task DeleteAsync<T, TId>(TId id, string collectionName = "")
             where TId : Id;
        Task<T> GetAsync<T, TId>(TId id, string collectionName = "")
             where TId : Id;
        Task<T> FindOneAsync<T, TId>(TId id, string collectionName = "", Action<Hit<T>> idAssignCallback = null);
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

        public async Task DeleteAsync<T, TId>(TId id, string collectionName = "")
            where TId : Id
        {
            var indexName = string.IsNullOrEmpty(collectionName) ? _repositoryHub.CollectionNameMapping.GetCollectionName<T>() : collectionName;
            var response = await _client.DeleteAsync(indexName, id);
            if (!response.IsValidResponse)
            {
                if (response.ApiCallDetails.OriginalException != null)
                    throw response.ApiCallDetails.OriginalException;
                else throw new InvalidOperationException(response.ApiCallDetails.ToString());
            }
        }

        public async Task<T> GetAsync<T, TId>(TId id, string collectionName = "")
            where TId : Id
        {
            var indexName = string.IsNullOrEmpty(collectionName) ? _repositoryHub.CollectionNameMapping.GetCollectionName<T>() : collectionName;
            var response = await _client.GetAsync<T>(id, idx => idx.Index(indexName));
            if (!response.IsValidResponse)
            {
                if (response.ApiCallDetails.OriginalException != null)
                    throw response.ApiCallDetails.OriginalException;
                else if (response.ElasticsearchServerError != null
                    && response.ElasticsearchServerError.Status == 404)
                {
                    throw new KeyNotFoundException($"index or doc can not be found");
                }
            }

            if (response.Source == null) throw new KeyNotFoundException(response.ApiCallDetails.ToString());
            return response.Source;
        }

        public async Task<T> FindOneAsync<T, TId>(TId id, string collectionName = "", Action<Hit<T>> idAssignCallback = null)
        {
            var indexName = string.IsNullOrEmpty(collectionName) ? _repositoryHub.CollectionNameMapping.GetCollectionName<T>() : collectionName;
            var request = new SearchRequest(indexName)
            {
                From = 0,
                Size = 10,
                Query = new TermQuery("id") { Value = id.ToString() }
            };
            var response = await _client.SearchAsync<T>(request);
            if (!response.IsValidResponse)
            {
                if (response.ApiCallDetails.OriginalException != null)
                    throw response.ApiCallDetails.OriginalException;
                else if (response.ElasticsearchServerError != null
                    && response.ElasticsearchServerError.Status == 404)
                {
                    throw new KeyNotFoundException($"index or doc can not be found");
                }
            }

            if (response.Documents == null) throw new KeyNotFoundException(response.ApiCallDetails.ToString());
            var docs = response.Hits.Select(h =>
              {
                  idAssignCallback?.Invoke(h);
                  return h.Source;
              });
            var result = docs.FirstOrDefault();
            if (result == null) return default;
            return result;
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
