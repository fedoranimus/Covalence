using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Covalence.Tests
{
    // [Collection("Integration")]
    public class TagsControllerIntegrationTests : IClassFixture<TestFixture<TestStartup>>
    {
        private readonly HttpClient Client;
        public TagsControllerIntegrationTests(TestFixture<TestStartup> fixture)
        { 
            Client = fixture.Client;
        }

        [Fact]
        public async Task GetAllTags() {
            var response = await Client.GetAsync("/api/tags");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<List<Tag>>(content);
            Assert.True(tags.Count == 3);
        }

        [Fact]
        public async Task CreateTag() {
            var tagName = "neurology";
            var uri = $"/api/tags/{tagName}";
            var response = await Client.PostAsync(uri, null);

            var testTag = new Tag() { Name = tagName };

            response.EnsureSuccessStatusCode();

            var allTagsResponse = await Client.GetAsync("/api/tags");
            allTagsResponse.EnsureSuccessStatusCode();

            var content = await allTagsResponse.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<List<Tag>>(content);
            Assert.True(tags.Count == 4);
            //Assert.Contains(testTag, tags);
        }

        [Theory]
        [InlineData("olo")]
        [InlineData("Chem")]
        [InlineData("chem")]
        public async Task QueryTag(string query) {
            var response = await Client.GetAsync($"/api/tags/query/{query}");

            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<List<Tag>>(content);
            Assert.True(tags.Count == 1);
        }
    }
}
