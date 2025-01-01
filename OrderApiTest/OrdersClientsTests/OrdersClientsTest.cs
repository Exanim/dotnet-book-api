using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OrderApi.Configurations;
using OrderApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrdersClientsTests
{
    public class OrdersClientsTest
    {
        Mock<IHttpClientWrapper> _httpClientWrapper;
        Mock<IOptions<ApiConfiguration>> _apiConfiguration;
        OrdersClients _ordersClients;

        [SetUp]
        public void SetUp()
        {
            _httpClientWrapper = new Mock<IHttpClientWrapper>();
            
            _apiConfiguration = new Mock<IOptions<ApiConfiguration>>();
            var apiConfiguration = new ApiConfiguration
            {
                UserApiUrl = "https://example.com/userapi",
                ProductApiUrl = "https://example.com/productapi"
            };

            _apiConfiguration.Setup(x => x.Value).Returns(apiConfiguration);

            _ordersClients = new OrdersClients(_apiConfiguration.Object, _httpClientWrapper.Object, _httpClientWrapper.Object);
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotFound)]
        public async Task GetUser(HttpStatusCode expectedStatusCode)
        {
            var expectedResponse = new HttpResponseMessage(expectedStatusCode);

            _httpClientWrapper.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expectedResponse);

            var response = await _ordersClients.GetUserAsync(1);

            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotFound)]
        public async Task GetProduct(HttpStatusCode expectedStatusCode)
        {
            var expectedResponse = new HttpResponseMessage(expectedStatusCode);

            _httpClientWrapper.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expectedResponse);

            var response = await _ordersClients.GetProductAsync(1);

            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }

    }
}
