namespace BetterCoding.MessagePubSubCenter.Repository
{
    public class CollectionNameAttribute: Attribute
    {
        public CollectionNameAttribute(string collectionName) => CollectionName = collectionName;

        public string CollectionName { get; private set; }
    }
}
