namespace BetterCoding.Strapi.SDK.Core
{
    public class StrapiWebhookPayload
    {
        public string Event { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Model { get; set; }
        public IDictionary<string, object> Entry { get; set; }
    }
}