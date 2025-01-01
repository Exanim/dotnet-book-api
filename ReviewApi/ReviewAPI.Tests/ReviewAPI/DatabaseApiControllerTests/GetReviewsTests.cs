using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Moq;
using NUnit.Framework;
using ReviewAPI.Controllers;
using ReviewAPI.Entities;
using ReviewAPI.Models;
using ReviewAPI.Services;
using ReviewAPI.Services.Caching;
using ReviewAPI.Services.Clients;

namespace Tests.ReviewAPI.DatabaseApiControllerTests
{
    internal class GetReviewsTests
    {
        Mock<IReviewRepository> _repositoryMock;
        Mock<IMapper> _mapperMock;
        DatabaseApiController _controller;
        List<Review> _exampleReviewEntities;
        List<ReviewDto> _exampleReviewDtos;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IReviewRepository>();
            _mapperMock = new Mock<IMapper>();
            _controller = new DatabaseApiController(
                Mock.Of<ILogger<DatabaseApiController>>(), 
                _mapperMock.Object,
                _repositoryMock.Object, 
                Mock.Of<IUserClient>(),
                Mock.Of<IProductClient>());

            _exampleReviewEntities = new List<Review>
                {
                    new Review()
                {
                    ReviewId = 1,
                    UserId = 15,
                    ProductId = 8,
                    ProductReview="asd"
                },
                new Review()
                {
                    ReviewId = 2,
                    UserId = 15,
                    ProductId = 1,
                    ProductReview="good"
                },
                new Review()
                {
                    ReviewId = 3,
                    UserId = 69,
                    ProductId = 8,
                    ProductReview="bad"
                }
                };

            _exampleReviewDtos = new List<ReviewDto>
                {
                    new ReviewDto()
                {
                    ReviewId = 1,
                    UserId = 15,
                    ProductId = 8,
                    ProductReview="asd"
                },
                new ReviewDto()
                {
                    ReviewId = 2,
                    UserId = 15,
                    ProductId = 1,
                    ProductReview="good"
                },
                new ReviewDto()
                {
                    ReviewId = 3,
                    UserId = 69,
                    ProductId = 8,
                    ProductReview="bad"
                }
                };

        }

        [Test]
        public async Task RequestsFullDb_ReturnsEveryDtoWithOK()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewsAsync(It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(_exampleReviewEntities) 
                .Verifiable();

            _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(_exampleReviewEntities))
                .Returns(_exampleReviewDtos) 
                .Verifiable();

            //Act
            ObjectResult response = (ObjectResult) await _controller.GetReviews(null, null);

            //Assert
            Assert.That(response.Value, Is.EqualTo(_exampleReviewDtos));
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();
            
        }

        [TestCase(69)]
        [TestCase(15)]
        public async Task UseUserIdFilter_ReturnOnlyReviewsFromUser(int userId)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewsAsync(userId, It.IsAny<int?>()))
                .ReturnsAsync(_exampleReviewEntities.FindAll(r => r.UserId == userId))
                .Verifiable();
            var _filteredReviews = await _repositoryMock.Object.GetReviewsAsync(userId, null);
            
            _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(_filteredReviews))
                .Returns(_exampleReviewDtos.FindAll(r => r.UserId==userId))
                .Verifiable();
            var _filteredReviewDtos = _mapperMock.Object.Map<IEnumerable<ReviewDto>>(_filteredReviews);
            //Act
            ObjectResult response = (ObjectResult)await _controller.GetReviews(userId, null);

            //Assert
            Assert.That(response.Value, Is.EqualTo(_filteredReviewDtos));
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }

        [TestCase(1)]
        [TestCase(8)]
        public async Task UseProductIdFilter_ReturnOnlyReviewsAboutProduct(int productId)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewsAsync(It.IsAny<int?>(), productId))
                .ReturnsAsync(_exampleReviewEntities.FindAll(r => r.ProductId == productId))
                .Verifiable();
            var _filteredReviews = await _repositoryMock.Object.GetReviewsAsync(null, productId);
            
            _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(_filteredReviews))
                .Returns(_exampleReviewDtos.FindAll(r => r.ProductId == productId))
                .Verifiable();
            var _filteredReviewDtos = _mapperMock.Object.Map<IEnumerable<ReviewDto>>(_filteredReviews);
            //Act
            ObjectResult response = (ObjectResult)await _controller.GetReviews(null, productId);

            //Assert
            Assert.That(response.Value, Is.EqualTo(_filteredReviewDtos));
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }

        [TestCase(69,8)]
        [TestCase(15,1)]
        public async Task UseBothFilters_ReturnExactReview(int userId, int productId)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewsAsync(userId,productId))
                .ReturnsAsync(_exampleReviewEntities.FindAll(r => r.UserId==userId && r.ProductId==productId))
                .Verifiable();
            var _filteredReviews = await _repositoryMock.Object.GetReviewsAsync(userId,productId);

            _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(_filteredReviews))
                .Returns(_exampleReviewDtos.FindAll(r => r.UserId == userId && r.ProductId == productId))
                .Verifiable();
            var _filteredReviewDtos = _mapperMock.Object.Map<IEnumerable<ReviewDto>>(_filteredReviews);
            //Act
            ObjectResult response = (ObjectResult)await _controller.GetReviews(userId, productId);

            //Assert
            Assert.That(response.Value, Is.EqualTo(_filteredReviewDtos));
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }

        [Test]
        public async Task FilterOutEverything_ReturnEmptyList()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewsAsync(It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new List<Review> { })
                .Verifiable();
            var _filteredReviews = await _repositoryMock.Object.GetReviewsAsync(null, null);
            
            _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(_filteredReviews))
                .Returns(new List<ReviewDto> { })
                .Verifiable();
            var _filteredReviewDtos = _mapperMock.Object.Map<IEnumerable<ReviewDto>>(_filteredReviews);
            //Act
            ObjectResult response = (ObjectResult)await _controller.GetReviews(null, null);

            //Assert
            Assert.That(response.Value, Is.Empty);
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }
    }
}
