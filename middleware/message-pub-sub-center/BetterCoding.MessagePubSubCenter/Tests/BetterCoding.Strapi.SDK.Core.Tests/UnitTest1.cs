using BetterCoding.Strapi.SDK.Core.Model;
using System.Text.Encodings.Web;
using System.Web;

namespace BetterCoding.Strapi.SDK.Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void QueryString_ShouldContainsArray_Test()
        {
            var bulder = new QueryBuilder().ContainedIn("id", new List<int> { 1, 2, 3 });
        }
    }
}