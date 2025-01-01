using RatingApi.Entities;
using Microsoft.EntityFrameworkCore;


namespace RatingApi.DbContexts;


public class RatingContext : DbContext
{

    public RatingContext(DbContextOptions<RatingContext> options) : base(options)
    {

    }
    
    public DbSet<Rating> Ratings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rating>().HasData(
            new Rating()
            {
                Id = 1,
                UserId = 321,
                ProductId = 3213,
                RatingValue = 5
            },
            new Rating()
            {
                Id = 2,
                UserId = 321,
                ProductId = 2001,
                RatingValue = 4
            },
            new Rating()
            {
                Id = 3,
                UserId = 4,
                ProductId = 512,
                RatingValue = 3
            },
            new Rating()
            {
                Id = 4,
                UserId = 51,
                ProductId = 92,
                RatingValue = 4
            }
        );
        base.OnModelCreating(modelBuilder);
    }

}