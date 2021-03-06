using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Xunit;


namespace FantasyXUnitTest
{
    public class T01_BasicTests : T00_WebTestBase
    {
        public T01_BasicTests(WebApplicationFactory<FantasyRestServer.Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async void Basic_ServerIsUp()
        {
            // Arrange
            HttpClient httpClient = this.WebFactory.CreateClient();

            // Act
            //string response = await client.GetStringAsync("https://localhost:44303/");
            HttpResponseMessage response = await httpClient.GetAsync("https://localhost:44303/");

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            string urlContents = await response.Content.ReadAsStringAsync();
            Assert.Equal("Welcome to Fantasy Manager Server!", urlContents);
        }
    }
}
