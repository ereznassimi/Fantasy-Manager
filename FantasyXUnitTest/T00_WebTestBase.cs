using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;


namespace FantasyXUnitTest
{
    public class T00_WebTestBase : IClassFixture<WebApplicationFactory<FantasyRestServer.Startup>>
    {
        protected readonly WebApplicationFactory<FantasyRestServer.Startup> WebFactory;

        public T00_WebTestBase(WebApplicationFactory<FantasyRestServer.Startup> factory)
        {
            this.WebFactory = factory;
        }
    }
}
