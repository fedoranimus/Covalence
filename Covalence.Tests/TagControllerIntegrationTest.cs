using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Covalence.Contracts;

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

        // [Fact]
        // public async Task GetAllTags() 
        // {
        //     var response = await Client.GetAsync("/api/tag");

        //     response.EnsureSuccessStatusCode();

        //     var content = await response.Content.ReadAsStringAsync();
        //     var tags = JsonConvert.DeserializeObject<List<TagContract>>(content);
        //     Assert.True(tags.Count == 0);
        // }

        [Fact]
        public async Task CreateTag() 
        {
            var tagName = "neurology";
            var uri = $"/api/tag/{tagName}";
            var response = await Client.PostAsync(uri, null);

            var testTag = new Tag() { Name = tagName };

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("neurology")]
        [InlineData("Neurology")]
        public async Task GetTag(string tagName)
        {
            var uri = $"/api/tag/{tagName}";
            var response = await Client.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tag = JsonConvert.DeserializeObject<TagContract>(content);
            Assert.Equal(tagName.ToLowerInvariant(), tag.Name);
        }

        [Theory]
        [InlineData("olo")]
        [InlineData("neu")]
        [InlineData("neu")]
        public async Task QueryTag(string query)
        {
            var response = await Client.GetAsync($"/api/tag/query/{query}");

            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<List<TagContract>>(content);
            Assert.True(tags.Count == 1);
        }
    }
}
