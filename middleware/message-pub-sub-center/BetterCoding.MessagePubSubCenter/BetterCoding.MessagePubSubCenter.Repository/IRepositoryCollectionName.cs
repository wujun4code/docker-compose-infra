using System.Reflection;

namespace BetterCoding.MessagePubSubCenter.Repository
{
    public interface IRepositoryCollectionNameMapping
    {
        string GetCollectionName<T>(T entity);
        string GetCollectionName<T>();
    }

    public class RepositoryCollectionNameMapping : IRepositoryCollectionNameMapping
    {
        public string GetCollectionName<T>(T entity)
        {
            return GetCollectionName<T>();
        }

        public string GetCollectionName<T>()
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttribute<CollectionNameAttribute>();
            if (attribute == null) throw new MissingFieldException(nameof(attribute));
            return attribute.CollectionName;
        }
    }
}
