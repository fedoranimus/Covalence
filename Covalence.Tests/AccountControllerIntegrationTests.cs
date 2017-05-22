using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System;
using Newtonsoft.Json;

namespace Covalence.Tests
{
    [Collection("Integration")]
    public class AccountControllerIntegrationTests
    {
        private readonly HttpClient Client;
        public AccountControllerIntegrationTests(TestFixture<TestStartup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task CreateAccount() 
        {
            var requestData = new { Email = "test12356@test.com", Password = "123Abc!", FirstName = "Test", LastName = "McTester", Location = "03062" };
            var body = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/account/register", body);

            response.EnsureSuccessStatusCode();
        }
    }
}
