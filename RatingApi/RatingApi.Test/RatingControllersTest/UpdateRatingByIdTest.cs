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

namespace Tests.RatingApi.RatingApiControllerTests
{
    internal class UpdateRatingByIdTest
    {
        Mock<IRatingRepository> _repositoryMock;
        Mock<IMapper> _mapperMock;
        RatingApiController _controller;
        List<Rating> _testRatingEntities;

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

            var reviewToUpdate= new RatingForUpdateDto 
            { 
                UserId=100,
                ProductId = 200,
                RatingValue = 1
            };

            _mapperMock.Setup(m => m.Map(reviewToUpdate, resultEntity))
                .Verifiable();

            //Act
            var response = (NoContentResult)await _controller.UpdateRatingById(Id, reviewToUpdate);

            //Assert
            Assert.That(response, Is.InstanceOf<NoContentResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }

        [Test]
        public async Task RequestInvalidId_BadRequestReturned()
        {
            //Arrange
            GenericErrorDto errorDto = new GenericErrorDto()
            {
                ErrorCode = GenericErrorDto.ErrorCodeEnum.InvalidRatingId,
                Message = "Invalid review ID! An ID has to be a positive integer"
            };

            //Act
            ObjectResult response = (ObjectResult)await _controller.GetRatingById(0);

            //Assert
            Assert.That(response.Value, Is.EqualTo(errorDto));
            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));

        }

        [TestCase(9999)]
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
            _mapperMock.Verify();

        }
    }
}
