using Moq.Protected;
using Moq;
using NUnit.Framework;
using System.Net;
using ReviewAPI.Services.Clients;
using ReviewAPI.Configurations;
using Microsoft.Extensions.Options;
using ReviewAPI.Services.Caching;
using Microsoft.Extensions.Logging;

namespace Tests.ReviewAPI.ReviewClientsTests
{
    internal class ProductClientTests
    {
        Mock<IHttpClientWrapper> _httpClientWrapperMock;
        Mock<ICache> _cacheMock;
        ProductClient _client;

        [SetUp]
        public void SetUp()
        {
            _httpClientWrapperMock = new Mock<IHttpClientWrapper>(MockBehavior.Strict);
            _httpClientWrapperMock
                .Setup( h=> h.setBaseAddress(It.IsAny<string>()))
                .Verifiable();
            //We have to setup _httpClientWrapperMock.GetAsync on a test-by-test basis
            _cacheMock = new Mock<ICache>(MockBehavior.Strict);
            //We have to setup _cacheMock.IsCached on a test-by-test basis
            _cacheMock
                .Setup(c => c.Add(It.IsAny<CacheType>(), It.IsAny<int>(), It.IsAny<Object>()))
                .Verifiable();


            _client = new ProductClient(
                _httpClientWrapperMock.Object,
                Options.Create(new ApiConfiguration()
                {
                    ProductApiUrl = "asdasd",
                    UserApiUrl = "dsadsa"
                }),
                _cacheMock.Object,
                Mock.Of<ILogger<ProductClient>>());

        }


        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotFound)]
        public async Task ProductInCache_ProductFoundInCacheAndReturnTrue(HttpStatusCode statusCode)
        {
            //Arrange
            _cacheMock
                .Setup(c => c.IsCached(It.IsAny<CacheType>(), It.IsAny<int>()))
                .Returns(true)
                .Verifiable();

            _httpClientWrapperMock
                .Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(statusCode))
                .Verifiable();

            //Act
            bool response = await _client.DoesProductExistAsync(1);
            //Assert
            Assert.That(response, Is.True);
            _cacheMock.Verify(
                c => c.IsCached(It.IsAny<CacheType>(), It.IsAny<int>()), 
                Times.Once);
            _httpClientWrapperMock.Verify(
                c => c.GetAsync(It.IsAny<string>()),
                Times.Never);
        }


        [Test]
        public async Task ProductNotInCacheButFoundOnServer_ProductFoundAndAddedToCache()
        {
            //Arrange
            _cacheMock
                .Setup(c => c.IsCached(It.IsAny<CacheType>(), It.IsAny<int>()))
                .Returns(false)
                .Verifiable();

            _httpClientWrapperMock
                .Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            //Act
            bool response = await _client.DoesProductExistAsync(1);
            //Assert
            Assert.That(response, Is.True);
            _cacheMock.Verify(
                c => c.IsCached(It.IsAny<CacheType>(), It.IsAny<int>()),
                Times.Once);
            _cacheMock.Verify(
                c => c.Add(It.IsAny<CacheType>(), It.IsAny<int>(), It.IsAny<object>()),
                Times.Once);
            _httpClientWrapperMock.Verify(
                c => c.GetAsync(It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        public async Task ProductNotInCacheOrOnServer_FalseReturnedCacheUpdated()
        {
            //Arrange
            _cacheMock
                .Setup(c => c.IsCached(It.IsAny<CacheType>(), It.IsAny<int>()))
                .Returns(false)
                .Verifiable();

            _httpClientWrapperMock
                .Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound))
                .Verifiable();

            //Act
            bool response = await _client.DoesProductExistAsync(1);
            //Assert
            Assert.That(response, Is.False);
            _cacheMock.Verify(
                c => c.IsCached(It.IsAny<CacheType>(), It.IsAny<int>()),
                Times.Once);
            _cacheMock.Verify(
                c => c.Add(It.IsAny<CacheType>(), It.IsAny<int>(), It.IsAny<object>()),
                Times.Once);
            _httpClientWrapperMock.Verify(
                c => c.GetAsync(It.IsAny<string>()),
                Times.Once);
        }

    }
}
