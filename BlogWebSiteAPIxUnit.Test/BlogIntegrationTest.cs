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
    public class BlogIntegrationTest
    {
        private readonly HttpClient _client;

        public BlogIntegrationTest()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = server.CreateClient();
        }


        [Fact]
        public void BlogGetAllTest()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/Blogs/GetBlogs");

            // Act
            var response = _client.SendAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("test")]
        public void BlogGetAllByUserTest(string strUserName)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/Blogs/GetBlogs/{strUserName}");

            // Act
            var response = _client.SendAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public void BlogGetAllByIdTest(int id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/Blogs/GetBlog/{id}");

            // Act
            var response = _client.SendAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void BlogPostTest()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/api/Blogs/");

            // Act
            var response = _client.SendAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public void BlogPutTest(int id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("PUT"), $"/api/Blogs/{id}");

            // Act
            var response = _client.SendAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public void BlogDeleteTest(int id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"/api/Blogs/{id}");

            // Act
            var response = _client.SendAsync(request).Result;

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }
}
