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

namespace Covalence.Tests {
    public class UserControllerIntegrationTest3 : IClassFixture<TestFixture<TestStartup>> {
        private readonly HttpClient Client;
        private readonly String Token;
        public UserControllerIntegrationTest3(TestFixture<TestStartup> fixture) {
            Client = fixture.Client;
            Token = fixture.Token;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }
        
        [Fact]
        public async Task AddTagToUser_AddSecondTag_ShouldContainTwoTags() {
            var tagType = "study";
            var tagName = "Physics";
            var uri = $"/api/user/tag/{tagType}/{tagName}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            var response = await Client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();


            var tagName2 = "Biology";
            var uri2 = $"/api/user/tag/{tagType}/{tagName2}";
            var requestMessage2 = new HttpRequestMessage(HttpMethod.Post, uri2);
            var response2 = await Client.SendAsync(requestMessage2);
            response.EnsureSuccessStatusCode();

            var userResponse = await Client.GetAsync("/api/user");
            response.EnsureSuccessStatusCode();

            var content = await userResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserContract>(content);

            Assert.Equal(2, user.StudyTags.Count());
        }
    }
}