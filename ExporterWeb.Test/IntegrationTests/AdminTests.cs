using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ExporterWeb.Test.IntegrationTests
{
    public class AdminTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public AdminTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/Admin/Users")]
        [InlineData("Admin/Users/Index")]
        [InlineData("/Admin/Users/Create")]
        [InlineData("/Admin/Users/Edit?id=fake")]
        public async Task Get_AdminPageRedirectsAnUnauthenticatedUser(string url)
        {
            // Arrange
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Identity/Account/Login",
                response.Headers.Location.LocalPath);
        }
    }
}
