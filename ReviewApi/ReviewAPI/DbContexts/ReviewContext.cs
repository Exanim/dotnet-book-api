using Microsoft.EntityFrameworkCore;
using ReviewAPI.Entities;

namespace ReviewAPI.DbContexts
{
    public class ReviewContext : DbContext
    {
        public virtual DbSet<Review> Reviews { get; set; } = null!;

        public ReviewContext(DbContextOptions<ReviewContext> options) 
            : base(options) 
        {
        
        }
        public ReviewContext()
            : base()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>().HasData(
                new Review()
                {
                    ReviewId = 1,
                    UserId = 15,
                    ProductId = 420,
                    ProductReview = "Overall good value for its price, but the battery life leaves much to be desired"
                },
                new Review()
                {
                    ReviewId = 2,
                    UserId = 26,
                    ProductId= 69,
                    ProductReview = "Very bad money grab"
                },
                new Review()
                {
                    ReviewId = 3,
                    UserId = 6,
                    ProductId = 76,
                    ProductReview = "Actually perfect, I can't recommend it enough"
                }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
