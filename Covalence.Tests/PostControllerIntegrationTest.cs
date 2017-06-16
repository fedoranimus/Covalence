using System.Net.Http;
using System.Threading.Tasks;
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
        }

        [Fact(Skip = "Not Implemented")]
        public async Task CreatePost() {
            Assert.True(false);
        }

        [Fact(Skip = "Not Implemented")]
        public async Task GetAllPosts() {
            Assert.True(false);
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