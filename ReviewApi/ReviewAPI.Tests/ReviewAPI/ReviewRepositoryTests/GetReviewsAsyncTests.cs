using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using ReviewAPI.DbContexts;
using ReviewAPI.Entities;
using ReviewAPI.Services;

namespace Tests.ReviewAPI.ReviewRepositoryTests
{
    public class GetReviewsAsyncTests
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
        public async Task NoParametersProvided_ReturnEverything()
        {
            //Arrange
            //The point of this test is having no input values, so no Arrange needed

            //Act
            IEnumerable<Review> everythingInDb = await _repository.GetReviewsAsync(null, null);
            //Assert
            Assert.That(everythingInDb.Count(),Is.EqualTo(3));

            int counter = 1;
            foreach(var review in everythingInDb) 
            {
                Assert.That(review.ReviewId, Is.EqualTo(counter));
                counter++;
            }
            _reviewContextMock.Verify();
        }

        [Test]
        public async Task FilterByUserIDOnly_UsersReviewsReturned()
        {
            //Arrange
            int userId = 15;

            //Act
            IEnumerable<Review> results = await _repository.GetReviewsAsync(userId,null);
            //Assert
            Assert.That(results.Count(), Is.EqualTo(2));

            foreach (var review in results)
            {
                Assert.That(review.UserId, Is.EqualTo(userId));
            }
            _reviewContextMock.Verify();
        }

        [Test]
        public async Task FilterByProductIDOnly_ProductsReviewsReturned()
        {
            //Arrange
            int productId = 8;

            //Act
            IEnumerable<Review> results = await _repository.GetReviewsAsync(null, productId);
            //Assert
            Assert.That(results.Count(), Is.EqualTo(2));

            foreach (var review in results)
            {
                Assert.That(review.ProductId, Is.EqualTo(productId));
            }
            _reviewContextMock.Verify();
        }

        [Test]
        public async Task FilterByProductAndUserID_ProductsReviewByUserReturned()
        {
            //Arrange
            int userId = 15;
            int productId = 8;

            //Act
            IEnumerable<Review> results = await _repository.GetReviewsAsync(userId, productId);
            //Assert
            Assert.That(results.Count(), Is.EqualTo(1));
            foreach (var review in results)
            {
                Assert.That(review.UserId, Is.EqualTo(userId));
                Assert.That(review.ProductId, Is.EqualTo(productId));
            }
            _reviewContextMock.Verify();
        }
        [Test]
        public async Task NonexistentIdProvided_EmptyListReturned()
        {
            //Arrange
            int userId = 1000;
            int productId = 8;

            //Act
            IEnumerable<Review> results = await _repository.GetReviewsAsync(userId, productId);
            //Assert
            CollectionAssert.IsEmpty(results);
            _reviewContextMock.Verify();
        }
    }
}