using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ReviewAPI.Controllers;
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
    internal class CreateReviewTests
    {
        Mock<IMapper> _mapperMock;
        Mock<IReviewRepository> _repositoryMock;
        Mock<IUserClient> _userClientMock;
        Mock<IProductClient> _productClientMock;
        DatabaseApiController _controller;

        /*
         private readonly ILogger<DatabaseApiController> _logger;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserClient _userClient;
        private readonly IProductClient _productClient;
         */
        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IReviewRepository>();
            _mapperMock = new Mock<IMapper>();
            _userClientMock = new Mock<IUserClient>();
            _productClientMock = new Mock<IProductClient>();
            _controller = new DatabaseApiController(
                Mock.Of<ILogger<DatabaseApiController>>(),
                _mapperMock.Object,
                _repositoryMock.Object,
                _userClientMock.Object,
                _productClientMock.Object);
        }

        [Test]
        public async Task BothUserAndProductExists_ReviewAdded()
        {
            //Arrange
            ReviewForCreationDto myReview = new ReviewForCreationDto();

            _userClientMock
                .Setup(u => u.DoesUserExistAsync(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _productClientMock
                .Setup(p => p.DoesProductExistAsync(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _mapperMock.Setup(m => m.Map<Review>(myReview))
                .Returns(new Review { })
                .Verifiable();
            var resultEntity = _mapperMock.Object.Map<Review>(myReview);

            _repositoryMock.Setup(r => r.AddReview(resultEntity)).Verifiable();
            _repositoryMock.Setup(r => r.SaveChangesAsync()).Verifiable();

            _mapperMock.Setup(m => m.Map<ReviewDto>(resultEntity))
                .Returns(new ReviewDto { })
                .Verifiable();
            var reviewToReturn = _mapperMock.Object.Map<ReviewDto>(resultEntity);
            //Act
            ObjectResult response = (ObjectResult)await _controller.CreateReview(myReview);

            //Assert
            Assert.That(response.Value, Is.EqualTo(reviewToReturn));
            Assert.That(response, Is.InstanceOf<CreatedAtRouteResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
            _repositoryMock.Verify();
            _mapperMock.Verify();
            _userClientMock.Verify();
            _productClientMock.Verify();

        }

        [Test]
        public async Task UserDoesntExist_NotFoundReturned()
        {
            //Arrange
            ReviewForCreationDto myReview = new ReviewForCreationDto();

            _userClientMock
                .Setup(u => u.DoesUserExistAsync(It.IsAny<int>()))
                .ReturnsAsync(false)
                .Verifiable();

            _productClientMock
                .Setup(p => p.DoesProductExistAsync(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            GenericErrorDto errorDto = new GenericErrorDto()
            {
                ErrorCode = GenericErrorDto.ErrorCodeEnum.UserNotFoundEnum,
                Message = "The user you are looking for is not found in the database."
            };

            //Act
            ObjectResult response = (ObjectResult)(await _controller.CreateReview(myReview));

            //Assert
            Assert.That(response.Value, Is.EqualTo(errorDto));
            Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            _userClientMock.Verify(x => x.DoesUserExistAsync(It.IsAny<int>()),Times.Once());
            _productClientMock.Verify(x => x.DoesProductExistAsync(It.IsAny<int>()), Times.Never());

        }

        [Test]
        public async Task ProductDoesntExist_NotFoundReturned()
        {
            //Arrange
            ReviewForCreationDto myReview = new ReviewForCreationDto();

            _userClientMock
                .Setup(u => u.DoesUserExistAsync(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _productClientMock
                .Setup(p => p.DoesProductExistAsync(It.IsAny<int>()))
                .ReturnsAsync(false)
                .Verifiable();

            GenericErrorDto errorDto = new GenericErrorDto()
            {
                ErrorCode = GenericErrorDto.ErrorCodeEnum.ProductNotFoundEnum,
                Message = "The product you are looking for is not found in the database."
            };

            //Act
            ObjectResult response = (ObjectResult)await _controller.CreateReview(myReview);

            //Assert
            //Assert
            Assert.That(response.Value, Is.EqualTo(errorDto));
            Assert.That(response, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            _userClientMock.Verify(x => x.DoesUserExistAsync(It.IsAny<int>()), Times.Once());
            _productClientMock.Verify(x => x.DoesProductExistAsync(It.IsAny<int>()), Times.Once());

        }


    }
}
