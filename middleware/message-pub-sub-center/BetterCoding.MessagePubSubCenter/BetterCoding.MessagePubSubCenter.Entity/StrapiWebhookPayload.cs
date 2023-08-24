namespace BetterCoding.MessagePubSubCenter.Entity
{
    public class StrapiWebhookPayload
    {
        public string Event { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Model { get; set; }
        public object Entry { get; set; }
    }
}