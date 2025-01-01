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
    internal class DeleteReviewTests
    {
        Mock<IReviewRepository> _repositoryMock;
        DatabaseApiController _controller;
        List<Review> _exampleReviewEntities;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IReviewRepository>(MockBehavior.Strict);
            _controller = new DatabaseApiController(
                Mock.Of<ILogger<DatabaseApiController>>(),
                Mock.Of<IMapper>(),
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

        }

        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public async Task ValidDeleteRequest_NoContentReturned(int reviewId)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewByIdAsync(reviewId))
                .ReturnsAsync(_exampleReviewEntities.FirstOrDefault(r => r.ReviewId == reviewId))
                .Verifiable();
            var resultEntity = await _repositoryMock.Object.GetReviewByIdAsync(reviewId);

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(true)
                .Verifiable();
            _repositoryMock.Setup(r => r.DeleteReview(resultEntity)).Verifiable();

            //Act
            var response = (NoContentResult)await _controller.DeleteReview(reviewId);

            //Assert
            Assert.That(response, Is.InstanceOf<NoContentResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
            _repositoryMock.Verify();

        }

        [Test]
        public async Task RequestInvalidId_BadRequestReturned()
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new InvalidIdException("asd",null))
                .Verifiable();

            //Act
            ObjectResult response = (ObjectResult)(await _controller.DeleteReview(0));
            //Assert
            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));

        }

        [TestCase(9999)]
        public async Task RequestNonexistentId_NotFoundReturned(int reviewId)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetReviewByIdAsync(reviewId))
                .ThrowsAsync(new ReviewNotFoundException("asd", null))
                .Verifiable();

            //Act
            ObjectResult response = (ObjectResult)await _controller.GetReviewById(reviewId);

            //Assert
            Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            _repositoryMock.Verify();

        }
    }
}
