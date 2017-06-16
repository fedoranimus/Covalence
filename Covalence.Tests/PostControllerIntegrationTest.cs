using System.Net.Http;
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
    }
}