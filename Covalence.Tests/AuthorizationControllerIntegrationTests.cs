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
    public class AuthorizationControllerIntegrationTests : IClassFixture<TestFixture<Covalence.Startup>>
    {
        public HttpClient Client { get; }

        public AuthorizationControllerIntegrationTests(TestFixture<Covalence.Startup> fixture)
        {
            Client = fixture.Client;
        }

        //TODO: Create a CollectionDefinition which saves the token to be available for authentication

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

            var loginBody = new FormUrlEncodedContent(keyValues);
            
            var loginResponse = await Client.PostAsync("/connect/token", loginBody);

            loginResponse.EnsureSuccessStatusCode();

            var loginResponseString = await loginResponse.Content.ReadAsStringAsync(); //read content, should be token

            var loginContent = JsonConvert.DeserializeObject<TokenResponse>(loginResponseString);

            //Console.WriteLine("{0}, {1}", loginContent.access_token.Length, loginContent.access_token);

            Assert.Equal(loginContent.token_type, "Bearer");
            Assert.True(loginContent.access_token.Length > 0);
        }
    }
}
