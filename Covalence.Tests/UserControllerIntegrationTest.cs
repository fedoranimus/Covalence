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

        // TODO: Write new user update tests

        // [Fact]
        // public async Task AddTagsToUser_CorrectData_ShouldContainThreeTags() {
        //     var getUser = await Client.GetAsync("/api/user");
        //     var getUserContent = await getUser.Content.ReadAsStringAsync();
        //     var userContent = JsonConvert.DeserializeObject<UserContract>(getUserContent);

        //     var userId = userContent.Id;
        //     var uri = $"/api/user/tags/{userId}";
        //     var requestContent = new string[] {"Physics", "biology", "dinology"};
        //     var body = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");
            
        //     var response = await Client.PutAsync(uri, body);

        //     response.EnsureSuccessStatusCode();

        //     // var userResponse = await Client.GetAsync("/api/user");
        //     // response.EnsureSuccessStatusCode();

        //     var content = await response.Content.ReadAsStringAsync();
        //     var user = JsonConvert.DeserializeObject<UserContract>(content);

        //     Assert.True(user.Tags.Count() == 3);       
        // }

        // [Fact]
        // public async Task RepeatAddTagsToUser_CorrectData_ShouldContainThreeTags() {
        //     var getUser = await Client.GetAsync("/api/user");
        //     var getUserContent = await getUser.Content.ReadAsStringAsync();
        //     var userContent = JsonConvert.DeserializeObject<UserContract>(getUserContent);

        //     var userId = userContent.Id;
        //     var uri = $"/api/user/tags/{userId}";
        //     var requestContent = new string[] {"Physics", "biology", "dinology"};
        //     var body = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");
            
        //     var response = await Client.PutAsync(uri, body);

        //     response.EnsureSuccessStatusCode();

        //     // var userResponse = await Client.GetAsync("/api/user");
        //     // response.EnsureSuccessStatusCode();

        //     var content = await response.Content.ReadAsStringAsync();
        //     var user = JsonConvert.DeserializeObject<UserContract>(content);

        //     Assert.True(user.Tags.Count() == 3);       
        // }



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
