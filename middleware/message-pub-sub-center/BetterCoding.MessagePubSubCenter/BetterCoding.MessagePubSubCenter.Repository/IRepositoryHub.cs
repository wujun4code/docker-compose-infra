namespace BetterCoding.MessagePubSubCenter.Repository
{
    public interface IRepositoryHub
    {
        IRepositoryCollectionNameMapping CollectionNameMapping { get; }
    }

    public class RepositoryHub : IRepositoryHub
    {
        public IRepositoryCollectionNameMapping CollectionNameMapping => new RepositoryCollectionNameMapping();
    }
}
