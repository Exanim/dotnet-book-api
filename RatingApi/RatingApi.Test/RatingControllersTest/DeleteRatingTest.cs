using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RatingApi.Controllers;
using RatingApi.Entities;
using RatingApi.Models;
using RatingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingApi.Test.RatingControllerTest
{
    internal class DeleteRatingTest
    {
        Mock<IRatingRepository> _repositoryMock;
        RatingApiController _controller;
        List<Rating> _testRatingEntities;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRatingRepository>();
            _controller = new RatingApiController(
                Mock.Of<ILogger<RatingApiController>>(),
                Mock.Of<IMapper>(),
                _repositoryMock.Object,
                Mock.Of<IRatingClients>());

            _testRatingEntities = new List<Rating>
                { 
                    new Rating()
                    {
                        Id = 1,
                        UserId = 15,
                        ProductId = 8,
                        RatingValue = 5
                    },
                    new Rating()
                    {
                        Id = 2,
                        UserId = 15,
                        ProductId = 1,
                        RatingValue = 4
                    },
                    new Rating()
                    {
                        Id = 3,
                        UserId = 69,
                        ProductId = 8,
                        RatingValue = 2
                    }
                };

        }

        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public async Task ValidPutRequest_NoContentReturned(int Id)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetRatingById(Id))
                .ReturnsAsync(_testRatingEntities.FirstOrDefault(r => r.Id == Id))
                .Verifiable();
            var resultEntity = await _repositoryMock.Object.GetRatingById(Id);

            _repositoryMock.Setup(r => r.SaveChangesAsync()).Verifiable();
            _repositoryMock.Setup(r => r.DeleteRating(resultEntity)).Verifiable();

            //Act
            var response = (NoContentResult)await _controller.DeleteRatingById(Id);

            //Assert
            Assert.That(response, Is.InstanceOf<NoContentResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
            _repositoryMock.Verify();

        }

        [Test]
        public async Task RequestInvalidId_BadRequestReturned()
        {
            //Arrange
            GenericErrorDto errorDto = new GenericErrorDto()
            {
                ErrorCode = GenericErrorDto.ErrorCodeEnum.InvalidRatingId,
                Message = "Invalid rating id! Id has to be positive integer"
            };

            //Act
            ObjectResult response = (ObjectResult)await _controller.GetRatingById(0);

            //Assert
            Assert.That(response.Value, Is.EqualTo(errorDto));
            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));

        }

        [TestCase(90)]
        public async Task RequestNonexistentId_NotFoundReturned(int Id)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetRatingById(Id))
                .ReturnsAsync(_testRatingEntities.FirstOrDefault(r => r.Id == Id))
                .Verifiable();
            var resultEntity = await _repositoryMock.Object.GetRatingById(Id);


            GenericErrorDto errorDto = new GenericErrorDto()
            {
                ErrorCode = GenericErrorDto.ErrorCodeEnum.RatingNotFoundEnum,
                Message = "Entry not found!"
            };

            //Act
            ObjectResult response = (ObjectResult)await _controller.GetRatingById(Id);

            //Assert
            Assert.That(resultEntity, Is.Null);
            Assert.That(response.Value, Is.EqualTo(errorDto));
            Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            _repositoryMock.Verify();

        }
    }
}
