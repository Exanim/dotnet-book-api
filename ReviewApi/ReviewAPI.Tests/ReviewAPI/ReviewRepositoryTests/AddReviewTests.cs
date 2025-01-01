using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using ReviewAPI.DbContexts;
using ReviewAPI.Entities;
using ReviewAPI.Services;

namespace Tests.ReviewAPI.ReviewRepositoryTests
{
    internal class AddReviewTests
    {
        Mock<ReviewContext> _reviewContextMock;
        ReviewRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _reviewContextMock = new Mock<ReviewContext>();

            _reviewContextMock.Setup(c => c.Reviews.Add(It.IsAny<Review>())).Verifiable();

            _repository = new ReviewRepository(_reviewContextMock.Object);
        }

        [Test]
        public void ReviewProvided_ReviewAdded()
        {
            //Arrange
            Review exampleReview = new Review()
            {
                ReviewId = 33,
                UserId = 44,
                ProductId = 55,
                ProductReview = "good"
            };

            //Act
             _repository.AddReview(exampleReview);
            //Assert
            _reviewContextMock.Verify();
        }
    }
}
