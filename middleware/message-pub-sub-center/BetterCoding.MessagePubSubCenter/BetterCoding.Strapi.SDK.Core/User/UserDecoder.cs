using BetterCoding.Strapi.SDK.Core.Services;

namespace BetterCoding.Strapi.SDK.Core.User
{
    public interface IUserDecoder
    {
        StrapiUser Decode(IDictionary<string, object> dictionary, IServiceHub serviceHub);
    }

    public class UserDecoder : IUserDecoder
    {
        public StrapiUser Decode(IDictionary<string, object> dictionary, IServiceHub serviceHub)
        {
            return new StrapiUser
            {
                JwtToken = serviceHub.Decoder.Grab<string>(dictionary,"jwt",o => o.ToString()),
                Username = serviceHub.Decoder.Grab<string>(dictionary, "user.username", o => o.ToString()),
            };
        }
    }
}
