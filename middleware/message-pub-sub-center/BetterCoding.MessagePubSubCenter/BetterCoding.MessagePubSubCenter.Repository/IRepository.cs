namespace BetterCoding.MessagePubSubCenter.Repository
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity);
    }

    public interface IRepository 
    {
        Task<T> AddAsync<T>(T entity, string collectionName = "");
    }
}
