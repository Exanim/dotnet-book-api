using Moq;
using RatingApi.DbContexts;
using RatingApi.Entities;
using RatingApi.Services;

namespace Tests.RatingApi.RatingRepositoryTests
{
    internal class DeleteRatingTest
    {
        Mock<RatingContext> _reviewContextMock;
        RatingRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _reviewContextMock = new Mock<RatingContext>();

            _reviewContextMock.Setup(c => c.Ratings.Remove(It.IsAny<Rating>())).Verifiable();

            _repository = new RatingRepository(_reviewContextMock.Object);
        }

        [Test]
        public void RatingProvided_RatingDeleted()
        {
            //Arrange
            Rating exampleRating = new Rating() 
            {
                Id = 33,
                UserId = 44,
                ProductId = 55,
                RatingValue = 4
            };

            //Act
            _repository.DeleteRating(exampleRating);
            //Assert
            _reviewContextMock.Verify();
        }
    }
}
