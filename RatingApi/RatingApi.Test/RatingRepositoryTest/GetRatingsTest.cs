using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RatingApi.DbContexts;
using RatingApi.Entities;
using RatingApi.Services;

namespace Tests.RatingApi.RatingRepositoryTests
{
    public class GetRatingsTest
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
        public async Task NoParametersProvided_ReturnEverything()
        {
            //Arrange
            //The point of this test is having no input values, so no Arrange needed

            //Act
            IEnumerable<Rating> everythingInDb = await _repository.GetRatings(null, null);
            //Assert
            Assert.That(everythingInDb.Count(),Is.EqualTo(3));

            int counter = 1;
            foreach(var review in everythingInDb) 
            {
                Assert.That(review.Id, Is.EqualTo(counter));
                counter++;
            }
            _reviewContextMock.Verify();
        }

        [Test]
        public async Task FilterByUserIDOnly_UsersRatingsReturned()
        {
            //Arrange
            int userId = 15;

            //Act
            IEnumerable<Rating> results = await _repository.GetRatings(userId,null);
            //Assert
            Assert.That(results.Count(), Is.EqualTo(2));

            foreach (var review in results)
            {
                Assert.That(review.UserId, Is.EqualTo(userId));
            }
            _reviewContextMock.Verify();
        }

        [Test]
        public async Task FilterByProductIDOnly_ProductsRatingsReturned()
        {
            //Arrange
            int productId = 8;

            //Act
            IEnumerable<Rating> results = await _repository.GetRatings(null, productId);
            //Assert
            Assert.That(results.Count(), Is.EqualTo(2));

            foreach (var review in results)
            {
                Assert.That(review.ProductId, Is.EqualTo(productId));
            }
            _reviewContextMock.Verify();
        }

        [Test]
        public async Task FilterByProductAndUserID_ProductsRatingByUserReturned()
        {
            //Arrange
            int userId = 15;
            int productId = 8;

            //Act
            IEnumerable<Rating> results = await _repository.GetRatings(userId, productId);
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
            IEnumerable<Rating> results = await _repository.GetRatings(userId, productId);
            //Assert
            CollectionAssert.IsEmpty(results);
            _reviewContextMock.Verify();
        }
    }
}