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
    internal class GetRatingByIdTest
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

        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public async Task RequestExistingId_CorrectRatingReturned(int Id)
        {
            //Arrange
            _repositoryMock.Setup(r => r.GetRatingById(Id))
                .ReturnsAsync(_testRatingEntities.FirstOrDefault(r => r.Id == Id))
                .Verifiable();
            var resultEntity = await _repositoryMock.Object.GetRatingById(Id);

            _mapperMock.Setup(m => m.Map<RatingDto>(resultEntity))
                .Returns(_testRatingDtos.FirstOrDefault(r => r.Id == Id))
                .Verifiable();
            var resultDto = _mapperMock.Object.Map<RatingDto>(resultEntity);

            //Act
            ObjectResult response = (ObjectResult)await _controller.GetRatingById(Id);

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
