using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RatingApi.Entities;
using RatingApi.Models;
using RatingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RatingApi.Controllers;

namespace Tests.RatingApi.RatingApiControllerTests
{
    internal class PostRatingTest
    {
        Mock<IRatingRepository> _repositoryMock;
        Mock<IMapper> _mapperMock;
        Mock<IRatingClients> _clientsMock;
        RatingApiController _controller;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRatingRepository>();
            _mapperMock = new Mock<IMapper>();
            _clientsMock = new Mock<IRatingClients>();
            _controller = new RatingApiController(
                Mock.Of<ILogger<RatingApiController>>(),
                _mapperMock.Object,
                _repositoryMock.Object,
                _clientsMock.Object);
        }

        [Test]
        public async Task BothUserAndProductExists_RatingAdded()
        {
            //Arrange
            RatingForCreationDto myRating=new RatingForCreationDto();

            _clientsMock.Setup(c => c.GetUserAsync(It.IsAny<int>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK })
                .Verifiable();

            _clientsMock.Setup(c => c.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK })
                .Verifiable();
            

            _mapperMock.Setup(m => m.Map<Rating>(myRating))
                .Returns(new Rating { })
                .Verifiable();
            var resultEntity = _mapperMock.Object.Map<Rating>(myRating);

            _repositoryMock.Setup(r => r.AddRating(resultEntity)).Verifiable();
            _repositoryMock.Setup(r => r.SaveChangesAsync()).Verifiable();

            _mapperMock.Setup(m => m.Map<RatingDto>(resultEntity))
                .Returns(new RatingDto { })
                .Verifiable();
            var reviewToReturn = _mapperMock.Object.Map<RatingDto>(resultEntity);
            //Act
            ObjectResult response = (ObjectResult) await _controller.PostRating(myRating);

            //Assert
            Assert.That(response.Value, Is.EqualTo(reviewToReturn));
            Assert.That(response, Is.InstanceOf<CreatedAtRouteResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
            _clientsMock.Verify();
            _repositoryMock.Verify();
            _mapperMock.Verify();

        }

        [Test]
        public async Task UserDoesntExist_NotFoundReturned()
        {
            //Arrange
            RatingForCreationDto myRating = new RatingForCreationDto();

            _clientsMock.Setup(c => c.GetUserAsync(It.IsAny<int>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.NotFound })
                .Verifiable();

            _clientsMock.Setup(c => c.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK })
                .Verifiable();

            GenericErrorDto errorDto = new GenericErrorDto()
            {
                ErrorCode = GenericErrorDto.ErrorCodeEnum.UserNotFoundEnum,
                Message = "The user you are looking for is not found in the database."
            };

            //Act
            ObjectResult response = (ObjectResult) await _controller.PostRating(myRating);

            //Assert
            Assert.That(response.Value, Is.EqualTo(errorDto));
            Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            _clientsMock.Verify(x => x.GetUserAsync(It.IsAny<int>()),Times.Once());
            _clientsMock.Verify(x => x.GetProductAsync(It.IsAny<int>()), Times.Never());
        }

        [Test]
        public async Task ProductDoesntExist_NotFoundReturned()
        {
            //Arrange
            RatingForCreationDto myRating = new RatingForCreationDto();

            _clientsMock.Setup(c => c.GetUserAsync(It.IsAny<int>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK })
                .Verifiable();

            _clientsMock.Setup(c => c.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.NotFound })
                .Verifiable();

            GenericErrorDto errorDto = new GenericErrorDto()
            {
                ErrorCode = GenericErrorDto.ErrorCodeEnum.ProductNotFoundEnum,
                Message = "The product you are looking for is not found in the database."
            };

            //Act
            ObjectResult response = (ObjectResult) await _controller.PostRating(myRating);

            //Assert
            Assert.That(response.Value, Is.EqualTo(errorDto));
            Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            _clientsMock.Verify(x => x.GetUserAsync(It.IsAny<int>()), Times.Once());
            _clientsMock.Verify(x => x.GetProductAsync(It.IsAny<int>()), Times.Once());
        }



    }
}
