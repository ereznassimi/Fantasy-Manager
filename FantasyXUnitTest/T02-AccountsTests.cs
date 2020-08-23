using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;


namespace FantasyXUnitTest
{
    public class T02_AccountsTests : IClassFixture<WebApplicationFactory<FantasyRestServer.Startup>>
    {
        private readonly WebApplicationFactory<FantasyRestServer.Startup> WebFactory;

        public T02_AccountsTests(WebApplicationFactory<FantasyRestServer.Startup> factory)
        {
            WebFactory = factory;
        }

        [Fact]
        public async void Signup_Missing_Email()
        {
            // Arrange
            HttpClient httpClient = this.WebFactory.CreateClient();

            // Act
            var signup_noemail = new { noemail = "erez@fantasy.com", password = "abcde" };
            StringContent content = new StringContent(
                signup_noemail.ToString(),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response =
                await httpClient.PostAsync("https://localhost:44303/api/accounts/signup", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            // must check the exact error
        }
    }
}
