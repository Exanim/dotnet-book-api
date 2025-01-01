using Moq;
using Moq.Protected;
using NUnit.Framework;
using RatingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RatingApi.Test.RatingClientsTest
{
    internal class RatingClientTest
    {
        Mock<HttpMessageHandler> _messageMock;
        RatingClients _ratingClients;
        StringContent content;

        [Test]
        public async Task httpClientGetRequestSent_ReturnsCorrectResponse()
        {
            //Arrange
            content = new StringContent("Mock Api response");
            _messageMock = new Mock<HttpMessageHandler>();
            _messageMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = content
                });
            
            string baseAdress = "https://localhost:7112";

            Mock<HttpClient> _httpClient = new Mock<HttpClient>(_messageMock.Object);
            _httpClient.Object.BaseAddress = new Uri(baseAdress);


            _ratingClients = new RatingClients(_httpClient.Object, _httpClient.Object);

            //Act
            HttpResponseMessage response = await _ratingClients.GetUserAsync(1);
            //Assert
            Assert.That(response.StatusCode,Is.EqualTo(System.Net.HttpStatusCode.NotFound));
            Assert.That(response.Content, Is.EqualTo(content));
        }
        

    }
}
