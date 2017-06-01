using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Covalence.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Covalence.Tests {
    public class TestFixture<TStartup> : IDisposable where TStartup : class  
    {
        private readonly TestServer _server;
        private readonly ApplicationDbContext _context;

        private TestHttpMessageHandler _testMessageHandler = new TestHttpMessageHandler();

        public TestFixture()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services => {
                    var jwtOptions = new JwtBearerOptions
                    {
                        AutomaticAuthenticate = true,
                        AutomaticChallenge = true,
                        RequireHttpsMetadata = false,
                        Audience = "http://localhost:5000",
                        Authority = "http://localhost:5000",
                        TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = OpenIdConnectConstants.Claims.Name,
                            RoleClaimType = OpenIdConnectConstants.Claims.Role,
                            ValidateAudience = false //Only for testing!
                        },
                        BackchannelHttpHandler = _testMessageHandler
                    };
                    services.AddSingleton(jwtOptions);
                })
                //.UseEnvironment("Development")
                .UseStartup<TStartup>();
            _server = new TestServer(builder);
            var innerHttpMessageHandler = _server.CreateHandler();
            _testMessageHandler.WrappedMessageHandler = innerHttpMessageHandler;

            _context = _server.Host.Services.GetRequiredService<ApplicationDbContext>();

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:5000");

            Token = FetchToken();
        }

        public HttpClient Client { get; }
        public String Token { get; }

        public string FetchToken() {
            var registerRequestData = new { Email = "fixture@test.com", Password = "123Abc!", FirstName = "Fixture", LastName = "User", Location = "03062" };
            var registerBody = new StringContent(JsonConvert.SerializeObject(registerRequestData), Encoding.UTF8, "application/json");

            var registerResponse = Client.PostAsync("/api/account/register", registerBody);

            registerResponse.Result.EnsureSuccessStatusCode();

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("username", "fixture@test.com"));
            keyValues.Add(new KeyValuePair<string, string>("password", "123Abc!"));
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
            keyValues.Add(new KeyValuePair<string, string>("scope", "offline_access"));
            keyValues.Add(new KeyValuePair<string, string>("resource", "http://localhost:5000"));

            var loginBody = new FormUrlEncodedContent(keyValues);
            
            var loginResponse = Client.PostAsync("/connect/token", loginBody);

            loginResponse.Result.EnsureSuccessStatusCode();

            var loginResponseString = loginResponse.Result.Content.ReadAsStringAsync(); //read content, should be token

            var loginContent = JsonConvert.DeserializeObject<TokenResponse>(loginResponseString.Result);

            return loginContent.access_token;
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            Client.Dispose();
            _server.Dispose();  
        }
    }
}