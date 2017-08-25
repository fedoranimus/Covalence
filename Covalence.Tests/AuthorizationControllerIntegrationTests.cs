using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Covalence.Tests
{
    // [Collection("Integration")]
    public class AuthorizationControllerIntegrationTests : IClassFixture<TestFixture<TestStartup>>
    {
        private readonly HttpClient Client;
        public AuthorizationControllerIntegrationTests(TestFixture<TestStartup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task Login() 
        {
            var registerRequestData = new { Email = "test123@test.com", Password = "123Abc!", FirstName = "Test", LastName = "McTester", Location = "03062" };
            var registerBody = new StringContent(JsonConvert.SerializeObject(registerRequestData), Encoding.UTF8, "application/json");

            var registerResponse = await Client.PostAsync("/api/account/register", registerBody);

            registerResponse.EnsureSuccessStatusCode();

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("username", "test123@test.com"));
            keyValues.Add(new KeyValuePair<string, string>("password", "123Abc!"));
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
            keyValues.Add(new KeyValuePair<string, string>("scope", "offline_access"));
            keyValues.Add(new KeyValuePair<string, string>("resource", "http://localhost:5000"));

            var loginBody = new FormUrlEncodedContent(keyValues);
            
            var loginResponse = await Client.PostAsync("/connect/token", loginBody);

            loginResponse.EnsureSuccessStatusCode();

            var loginResponseString = await loginResponse.Content.ReadAsStringAsync(); //read content, should be token

            var loginContent = JsonConvert.DeserializeObject<TokenResponse>(loginResponseString);

            //Console.WriteLine("{0}, {1}", loginContent.access_token.Length, loginContent.access_token);

            Assert.Equal("Bearer", loginContent.token_type);
            Assert.True(loginContent.access_token.Length > 0);
        }
    }
}
