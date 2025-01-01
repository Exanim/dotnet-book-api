

using Books.Api.Entity;
using Microsoft.EntityFrameworkCore;
using Org.OpenAPITools.Models;

namespace Books.Api.DbContexts
{
    public class BooksContext : DbContext
    {
        public DbSet<BookEntity> Books { get; set; } = null!;

        public BooksContext(DbContextOptions<BooksContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookEntity>().HasData(
            new BookEntity("Test1")
            {
                Id = 1,
            },

            new BookEntity("Test2")
            {
                Id = 2,
            },

            new BookEntity("Test3")
            {
                Id = 3,
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
