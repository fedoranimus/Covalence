using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Xunit;

namespace Covalence.Tests {
    public class TestFixture<TStartup> : IAsyncLifetime where TStartup : class  
    {
        private TestServer _server;
        private ApplicationDbContext _context;

        public async Task InitializeAsync()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Staging")
                .UseStartup<TStartup>();
            _server = new TestServer(builder);
            _context = _server.Host.Services.GetRequiredService<ApplicationDbContext>();

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:5000");

            Token = await FetchToken();
        }

        public HttpClient Client { get; private set; }
        public String Token { get; private set; }

        public async Task<string> FetchToken() {
            var registerRequestData = new { Email = "fixture@test.com", Password = "123Abc!", FirstName = "Fixture", LastName = "User", Location = "03062" };
            var registerBody = new StringContent(JsonConvert.SerializeObject(registerRequestData), Encoding.UTF8, "application/json");

            var registerResponse = await Client.PostAsync("/api/account/register", registerBody);

            registerResponse.EnsureSuccessStatusCode();

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("username", "fixture@test.com"));
            keyValues.Add(new KeyValuePair<string, string>("password", "123Abc!"));
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
            keyValues.Add(new KeyValuePair<string, string>("scope", "offline_access"));
            keyValues.Add(new KeyValuePair<string, string>("resource", "http://localhost:5000"));

            var loginBody = new FormUrlEncodedContent(keyValues);
            
            var loginResponse = await Client.PostAsync("/connect/token", loginBody);

            loginResponse.EnsureSuccessStatusCode();

            var loginResponseString = loginResponse.Content.ReadAsStringAsync(); //read content, should be token

            var loginContent = JsonConvert.DeserializeObject<TokenResponse>(loginResponseString.Result);

            return loginContent.access_token;
        }

        public Task DisposeAsync()
        {
            _context.Database.EnsureDeleted();
            Client.Dispose();
            _server.Dispose();  
            return Task.CompletedTask;
        }
    }
}