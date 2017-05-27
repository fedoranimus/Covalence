using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Covalence.Contracts;
using System.Linq;

namespace Covalence.Tests
{
    // [Collection("Integration")]
    public class UserControllerIntegrationTests : IClassFixture<TestFixture<TestStartup>>
    {
        private readonly HttpClient Client;
        private readonly String Token;
        public UserControllerIntegrationTests(TestFixture<TestStartup> fixture)
        {
            Client = fixture.Client;
            Token = fixture.Token;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }

        [Fact]
        public async Task GetUser() {
            var response = await Client.GetAsync("/api/user");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserContract>(content);

            Assert.Equal("fixture@test.com", user.Email);
        }

        [Fact]
        public async Task AddTagToUser_CorrectData_ShouldContainSingleTag() {
            var tagType = "study";
            var tagName = "Physics";
            var uri = $"/api/user/tag/{tagType}/{tagName}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            var response = await Client.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();

            var userResponse = await Client.GetAsync("/api/user");
            response.EnsureSuccessStatusCode();

            var content = await userResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserContract>(content);

            Assert.Equal("Physics", user.StudyTags.Single().Name);         
        }

        [Fact]
        public async Task AddTagToUser_IncorrectTagType_ShouldReturn400Error() {
            var tagType = "dinosaurs"; // Testing non-accepted tagtypes
            var tagName = "Physics";
            var uri = $"/api/user/tag/{tagType}/{tagName}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            var response = await Client.SendAsync(requestMessage);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Invalid tag type", content);
        }

        [Fact]
        public async Task AddTagToUser_NonexistentTag_ShouldReturn400Error() {
            var tagType = "study";
            var tagName = "Eatology";
            var uri = $"/api/user/tag/{tagType}/{tagName}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            var response = await Client.SendAsync(requestMessage);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal($"No tag corresponding to '{tagName}'", content);
        }

        // [Fact(Skip="Not Implemented")]
        // public async Task RequestConnectionToUser() {

        // }

        // [Fact(Skip="Not Implemented")]
        // public async Task AcceptConnectionFromUser() {

        // }

        // [Fact(Skip="Not Implemented")]
        // public async Task RejectConnectionFromUser() {

        // }

        // [Fact(Skip="Not Implemented")]
        // public async Task BlockUser() {

        //}
    }
}
