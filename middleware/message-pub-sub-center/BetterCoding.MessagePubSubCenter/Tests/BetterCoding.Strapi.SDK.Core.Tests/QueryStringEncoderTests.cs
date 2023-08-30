using BetterCoding.Strapi.SDK.Core.Query;
using Newtonsoft.Json.Linq;

namespace BetterCoding.Strapi.SDK.Core.Tests
{
    public class QueryStringEncoderTests
    {
        [Fact]
        public void QueryString_ShouldEqual_Test()
        {
            var client = new StrapiClient();
            var actual = client.Services.QueryStringEncoder.Encode(7, "$eq");
            Assert.Equal("[$eq]=7", actual);
        }

        [Fact]
        public void QueryString_ShouldFieldEqual_Test()
        {
            var client = new StrapiClient();
            var actual = client.Services.QueryStringEncoder.Encode(7, "id", "$eq");
            Assert.Equal("id[$eq]=7", actual);
        }

        [Fact]
        public void QueryString_ShouldEncodeFiltersIdEqual_Test()
        {
            var client = new StrapiClient();

            var actual = client.Services.QueryStringEncoder.Encode(7, "filters", "id", "$eq");
            Assert.Equal("filters[id][$eq]=7", actual);
        }

        [Fact]
        public void QueryString_ShouldEncode_Dictionary_IdEqual_Test()
        {
            var client = new StrapiClient();
            var idEqual = new Dictionary<string, object> { { "id", new Dictionary<string, object> { { "$eq", 7 } } } };
            var actual = client.Services.QueryStringEncoder.Encode(idEqual);
            Assert.Equal("id[$eq]=7", actual);
        }

        [Fact]
        public void QueryString_ShouldEncode_Dictionary_MultipleEquals_Test()
        {
            var client = new StrapiClient();

            var multipleEquals = new Dictionary<string, object> {
                { "id", new Dictionary<string, object> { { "$eq", 7 } } },
                { "name", new Dictionary<string, object> { { "$eq", "test" } } }
            };

            var filters = new Dictionary<string, object>
            {
                { "filters",multipleEquals }
            };

            var actual = client.Services.QueryStringEncoder.Encode(filters);
            Assert.Equal("filters[id][$eq]=7&filters[name][$eq]=test", actual);
        }

        [Fact]
        public void QueryString_ShouldEncode_Dictionary_IdInArray_Test()
        {
            var client = new StrapiClient();

            var idInArrray = new Dictionary<string, object> {
                { "id", new Dictionary<string, object> { { "$eq", new List<object> {1,2,3 } } } },
            };

            var filters = new Dictionary<string, object>
            {
                { "filters", idInArrray }
            };

            var actual = client.Services.QueryStringEncoder.Encode(filters);
            Assert.Equal("filters[id][$eq]=7&filters[name][$eq]=test", actual);
        }
    }
}
