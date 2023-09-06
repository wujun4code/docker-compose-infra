namespace BetterCoding.Strapi.SDK.Core.User
{
    public class StrapiUser
    {
        public string JwtToken { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Provider { get; set; }
        public bool Confirmed { get; set; }
        public bool Blocked { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
