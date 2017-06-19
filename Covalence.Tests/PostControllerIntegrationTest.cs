using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Covalence.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace Covalence.Tests {
    public class PostControllerIntegrationTests : IClassFixture<TestFixture<TestStartup>>
    {
        private readonly HttpClient Client;
        private readonly string Token;
        public PostControllerIntegrationTests(TestFixture<TestStartup> fixture)
        { 
            Client = fixture.Client;
            Token = fixture.Token;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }

        [Fact]
        public async Task CreatePost() {
            var uri = $"/api/post";
            var tagList = new List<string>(){ "physics", "biology"};
            PostViewModel model = new PostViewModel(){ Title = "Test Title", Content = "abcdefghijk", Tags = tagList, Category = 1 };
            var postContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(uri, postContent);

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPost(int postId) {
            var uri = $"/api/post/{postId}";
            var response = await Client.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<Post>(content);
            Assert.Equal(postId, post.PostId);
            Assert.Equal("Test Title", post.Title);
            Assert.Equal("abcdefghijk", post.Content);
        }

        [Fact(Skip = "Not Implemented")]
        public async Task CreateTagsWithPost() {
            Assert.True(false);
        }

        [Fact]
        public async Task GetAllPosts() {
            var uri = $"/api/post";
            var response = await Client.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<List<Post>>(content);
            Assert.True(posts.Count == 1);
        }

        [Fact(Skip = "Not Implemented")]
        public async Task DeletePost() {
            Assert.True(false);
        }

        [Fact(Skip = "Not Implemented")]
        public async Task AddTagToPost() {
            Assert.True(false);
        }

        [Fact(Skip = "Not Implemented")]
        public async Task RemoveTagFromPost() {
            Assert.True(false);
        }
    }
}