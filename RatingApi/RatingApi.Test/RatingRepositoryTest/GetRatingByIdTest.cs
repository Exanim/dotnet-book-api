using Moq;
using NUnit.Framework;
using RatingApi.DbContexts;
using RatingApi.Entities;
using RatingApi.Services;

namespace Tests.RatingApi.RatingRepositoryTests
{
    public class GetRatingByIdTest
    {
        Mock<RatingContext> _reviewContextMock;
        RatingRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _reviewContextMock = new Mock<RatingContext>();

            _reviewContextMock.Setup(r => r.Ratings).ReturnsDbSet(new List<Rating>()
            {
                new Rating()
                {
                    Id = 1,
                    UserId = 15,
                    ProductId = 8,
                    RatingValue = 4
                },
                new Rating()
                {
                    Id = 2,
                    UserId = 15,
                    ProductId = 1,
                    RatingValue = 3
                },
                new Rating()
                {
                    Id = 3,
                    UserId = 69,
                    ProductId = 8,
                    RatingValue = 1
                }
            })
                .Verifiable();

            _repository = new RatingRepository(_reviewContextMock.Object);
        }

        [Test]
        public async Task ValidIdProvided_RatingReturned()
        {
            //Arrange
            int reviewId = 2;

            //Act
            Rating result = await _repository.GetRatingById(reviewId);
            //Assert
            Assert.That(result.Id, Is.EqualTo(2));
            _reviewContextMock.Verify();
        }

        [Test]
        public async Task NonExistentIdProvided_RatingReturned()
        {
            //Arrange
            int reviewId = 2000;

            //Act
            Rating result = await _repository.GetRatingById(reviewId);
            //Assert
            Assert.That(result, Is.Null);
            _reviewContextMock.Verify();
        }

    }
}
