using Elastic.Clients.Elasticsearch;

namespace BetterCoding.MessagePubSubCenter.Repository
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity);
    }

    public interface IRepository
    {

    }
}
