using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Moq;
using NUnit.Framework;
using RatingApi.Controllers;
using RatingApi.Entities;
using RatingApi.Models;
using RatingApi.Services;

namespace Tests.RatingApi.RatingApiControllerTests
{
    internal class GetRatingsTest
    {
        Mock<IRatingRepository> _repositoryMock;
        Mock<IMapper> _mapperMock;
        RatingApiController _controller;
        List<Rating> _testRatingEntities;
        List<RatingDto> _testRatingDtos;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRatingRepository>();
            _mapperMock = new Mock<IMapper>();
            _controller = new RatingApiController(
                Mock.Of<ILogger<RatingApiController>>(), 
                _mapperMock.Object,
                _repositoryMock.Object, 
                Mock.Of<IRatingClients>());

            _testRatingEntities = new List<Rating>
            {
                new Rating()
                {
                    Id = 1,
                    UserId = 32,
                    ProductId = 8,
                    RatingValue = 4
                },
                new Rating()
                {
                    Id = 2,
                    UserId = 44,
                    ProductId = 1,
                    RatingValue = 5
                },
                new Rating()
                {
                    Id = 3,
                    UserId = 90,
                    ProductId = 12,
                    RatingValue = 2
                }
            };

            _testRatingDtos = new List<RatingDto>
            {
                new RatingDto()
                {
                    Id = 1,
                    UserId = 32,
                    ProductId = 8,
                    RatingValue = 4
                },
                new RatingDto()
                {
                    Id = 2,
                    UserId = 44,
                    ProductId = 1,
                    RatingValue = 5
                },
                new RatingDto()
                {
                    Id = 3,
                    UserId = 90,
                    ProductId = 12,
                    RatingValue = 2
                }
            };

        }

        [Test]
        public async Task RequestsFullDb_ReturnsEveryDtoWithOK()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetRatings(It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(_testRatingEntities) 
                .Verifiable();

            _mapperMock.Setup(m => m.Map<IEnumerable<RatingDto>>(_testRatingEntities))
                .Returns(_testRatingDtos) 
                .Verifiable();

            //Act
            ObjectResult response = (ObjectResult) await _controller.GetRatings(null, null);

            //Assert
            Assert.That(response.Value, Is.EqualTo(_testRatingDtos));
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();
            
        }

        [TestCase(69)]
        [TestCase(15)]
        public async Task UseUserIdFilter_ReturnOnlyRatingsFromUser(int userId)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetRatings(userId, It.IsAny<int?>()))
                .ReturnsAsync(_testRatingEntities.FindAll(r => r.UserId == userId))
                .Verifiable();
            var _filteredRatings = await _repositoryMock.Object.GetRatings(userId, null);
            
            _mapperMock.Setup(m => m.Map<IEnumerable<RatingDto>>(_filteredRatings))
                .Returns(_testRatingDtos.FindAll(r => r.UserId==userId))
                .Verifiable();
            var _filteredRatingDtos = _mapperMock.Object.Map<IEnumerable<RatingDto>>(_filteredRatings);
            //Act
            ObjectResult response = (ObjectResult) await _controller.GetRatings(userId, null);

            //Assert
            Assert.That(response.Value, Is.EqualTo(_filteredRatingDtos));
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }

        [TestCase(1)]
        [TestCase(8)]
        public async Task UseProductIdFilter_ReturnOnlyRatingsAboutProduct(int productId)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetRatings(It.IsAny<int?>(), productId))
                .ReturnsAsync(_testRatingEntities.FindAll(r => r.ProductId == productId))
                .Verifiable();
            var _filteredRatings = await _repositoryMock.Object.GetRatings(null, productId);
            
            _mapperMock.Setup(m => m.Map<IEnumerable<RatingDto>>(_filteredRatings))
                .Returns(_testRatingDtos.FindAll(r => r.ProductId == productId))
                .Verifiable();
            var _filteredRatingDtos = _mapperMock.Object.Map<IEnumerable<RatingDto>>(_filteredRatings);
            //Act
            ObjectResult response = (ObjectResult) await _controller.GetRatings(null, productId);

            //Assert
            Assert.That(response.Value, Is.EqualTo(_filteredRatingDtos));
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }

        [TestCase(69,8)]
        [TestCase(15,1)]
        public async Task UseBothFilters_ReturnExactRating(int userId, int productId)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetRatings(userId,productId))
                .ReturnsAsync(_testRatingEntities.FindAll(r => r.UserId==userId && r.ProductId==productId))
                .Verifiable();
            var _filteredRatings = await _repositoryMock.Object.GetRatings(userId,productId);

            _mapperMock.Setup(m => m.Map<IEnumerable<RatingDto>>(_filteredRatings))
                .Returns(_testRatingDtos.FindAll(r => r.UserId == userId && r.ProductId == productId))
                .Verifiable();
            var _filteredRatingDtos = _mapperMock.Object.Map<IEnumerable<RatingDto>>(_filteredRatings);
            //Act
            ObjectResult response = (ObjectResult) await _controller.GetRatings(userId, productId);

            //Assert
            Assert.That(response.Value, Is.EqualTo(_filteredRatingDtos));
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }

        [Test]
        public async Task FilterOutEverything_ReturnEmptyList()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetRatings(It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new List<Rating> { })
                .Verifiable();
            var _filteredRatings = await _repositoryMock.Object.GetRatings(null, null);
            
            _mapperMock.Setup(m => m.Map<IEnumerable<RatingDto>>(_filteredRatings))
                .Returns(new List<RatingDto> { })
                .Verifiable();
            var _filteredRatingDtos = _mapperMock.Object.Map<IEnumerable<RatingDto>>(_filteredRatings);
            //Act
            ObjectResult response = (ObjectResult) await _controller.GetRatings(null, null);

            //Assert
            Assert.That(response.Value, Is.Empty);
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }
    }
}
