namespace BetterCoding.MessagePubSubCenter.Entity.Configurations
{
    public class RabbitMQ
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string Vhost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
