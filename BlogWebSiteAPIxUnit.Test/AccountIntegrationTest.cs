using BlogWebSiteAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace BlogWebSiteAPIxUnit.Test
{
    public class AccountIntegrationTest
    {
        private readonly HttpClient _client;

        public AccountIntegrationTest()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        public void LoginPostTest()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/api/Account/login");

            // Act
            var response = _client.SendAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
