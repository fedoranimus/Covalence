using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Covalence.Contracts;
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

            // var uri2 = $"/api/post/1";
            // var response2 = await Client.GetAsync(uri2);

            // response2.EnsureSuccessStatusCode();
            // var content = await response2.Content.ReadAsStringAsync();
            // var post = JsonConvert.DeserializeObject<PostContract>(content);

            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<PostContract>(content);
            Assert.Equal(1, post.PostId);
            Assert.Equal("Test Title", post.Title);
            Assert.Equal("abcdefghijk", post.Content);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPost(int postId) {
            var uri = $"/api/post/{postId}";
            var response = await Client.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<PostContract>(content);
            Assert.Equal(postId, post.PostId);
            Assert.Equal("Test Title", post.Title);
            Assert.Equal("abcdefghijk", post.Content);
        }

        [Fact]
        public async Task CreateTagsWithPost() {
            var uri = $"/api/post";
            var tagList = new List<string>(){ "physics", "biology", "exobiology"};
            PostViewModel model = new PostViewModel(){ Title = "Test Title", Content = "abcdefghijk", Tags = tagList, Category = 1 };
            var postContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        
            var response = await Client.PostAsync(uri, postContent);

            response.EnsureSuccessStatusCode();
        }

        [Fact(Skip = "Not Implemented")]
        public async Task GetAllPosts() {
            var uri = $"/api/post";
            var response = await Client.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<List<PostContract>>(content);
            Console.WriteLine(posts.Count);
            Assert.True(posts.Count == 2);
        }


        [Theory(Skip = "Not Implemented")]
        [InlineData(1)]
        public async Task AddTagToPost(int postId) {
            var uri = $"/api/post/{postId}";
            var tagList = new List<string>(){ "physics", "biology", "chemistry"};
            PostViewModel model = new PostViewModel(){ Title = "Test Title", Content = "abcdefghijk", Tags = tagList, Category = 1 };
            var postContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            
            var response = await Client.PutAsync(uri, postContent);

            response.EnsureSuccessStatusCode();
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(1)]
        public async Task RemoveTagFromPost(int postId) {

        }

        [Theory]
        [InlineData(1)]
        public async Task DeletePost(int postId) {
            var uri = $"/api/post/{postId}";
            var response = await Client.DeleteAsync(uri);

            response.EnsureSuccessStatusCode();
        }
    }
}