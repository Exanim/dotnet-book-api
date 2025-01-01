using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using ReviewAPI.CustomErrors;
using ReviewAPI.DbContexts;
using ReviewAPI.Entities;
using ReviewAPI.Services;

namespace Tests.ReviewAPI.ReviewRepositoryTests
{
    public class GetReviewByIdAsyncTests
    {
        Mock<ReviewContext> _reviewContextMock;
        ReviewRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _reviewContextMock = new Mock<ReviewContext>();

            _reviewContextMock.Setup(r => r.Reviews).ReturnsDbSet(new List<Review>()
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
            })
                .Verifiable();

            _repository = new ReviewRepository(_reviewContextMock.Object);
        }

        [Test]
        public async Task ValidIdProvided_ReviewReturned()
        {
            //Arrange
            int reviewId = 2;

            //Act
            Review result = await _repository.GetReviewByIdAsync(reviewId);
            //Assert
            Assert.That(result.ReviewId, Is.EqualTo(2));
            _reviewContextMock.Verify();
        }

        [Test]
        public async Task NonExistentIdProvided_NotFoundExceptionThrown()
        {
            //Arrange
            int reviewId = 2000;

            //Act
            //Assert
            Assert.ThrowsAsync<ReviewNotFoundException>(async () => await _repository.GetReviewByIdAsync(reviewId));
            _reviewContextMock.Verify();
        }

        [Test]
        public async Task MalformedIdProvided_InvalidIdExceptionThrown()
        {
            //Arrange
            int reviewId = -1;

            //Act
            //Assert
            Assert.ThrowsAsync<InvalidIdException>(async () => await _repository.GetReviewByIdAsync(reviewId));
        }

    }
}
