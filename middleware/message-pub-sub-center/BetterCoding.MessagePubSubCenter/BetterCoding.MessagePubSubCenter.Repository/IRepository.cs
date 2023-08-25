namespace BetterCoding.MessagePubSubCenter.Repository
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity);
    }
}
