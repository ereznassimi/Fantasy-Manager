using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;



namespace FantasyXUnitTest
{
    public class T02_AccountsTests : T00_WebTestBase
    {
        public T02_AccountsTests(WebApplicationFactory<FantasyRestServer.Startup> factory)
            : base(factory)
        {
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

            HttpResponseMessage httpResponse =
                await httpClient.PostAsync("https://localhost:44303/api/accounts/signup", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            // must check the exact error
            string responseBody = await httpResponse.Content.ReadAsStringAsync();
            dynamic response = JsonConvert.DeserializeObject(responseBody);

            int status = int.Parse(response["status"].ToString());
            Assert.Equal((int)HttpStatusCode.BadRequest, status);
        }
    }
}
