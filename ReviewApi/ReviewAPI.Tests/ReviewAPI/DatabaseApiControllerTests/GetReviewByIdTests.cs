using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ReviewAPI.Controllers;
using ReviewAPI.CustomErrors;
using ReviewAPI.Entities;
using ReviewAPI.Models;
using ReviewAPI.Services;
using ReviewAPI.Services.Caching;
using ReviewAPI.Services.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ReviewAPI.DatabaseApiControllerTests
{
    internal class GetReviewByIdTests
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

        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public async Task RequestExistingId_CorrectReviewReturned(int reviewId)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewByIdAsync(reviewId))
                .ReturnsAsync(_exampleReviewEntities.FirstOrDefault(r => r.ReviewId == reviewId))
                .Verifiable();
            var resultEntity = await _repositoryMock.Object.GetReviewByIdAsync(reviewId);

            _mapperMock.Setup(m => m.Map<ReviewDto>(resultEntity))
                .Returns(_exampleReviewDtos.FirstOrDefault(r => r.ReviewId == reviewId))
                .Verifiable();
            var resultDto = _mapperMock.Object.Map<ReviewDto>(resultEntity);

            //Act
            ObjectResult response = (ObjectResult)await _controller.GetReviewById(reviewId);

            //Assert
            Assert.That(response.Value, Is.EqualTo(resultDto));
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }

        [Test]
        public async Task RequestInvalidId_BadRequestReturned()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new InvalidIdException("asd", null))
                .Verifiable();
            //Act
            ObjectResult response = (ObjectResult)await (_controller.GetReviewById(0));

            //Assert
            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            _repositoryMock.Verify();
        }

        [TestCase(9999)]
        public async Task RequestNonexistentId_NotFoundReturned(int reviewId)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewByIdAsync(reviewId))
                .ThrowsAsync(new ReviewNotFoundException("asd", null))
                .Verifiable();

            //Act
            ObjectResult response = (ObjectResult)(await _controller.GetReviewById(reviewId));

            //Assert
            Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            _repositoryMock.Verify();

        }
    }
}
