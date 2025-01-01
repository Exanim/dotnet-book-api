using Microsoft.EntityFrameworkCore;
using userAPI.Entities;

namespace userAPI.DbContexts
{
    public class UserAPIContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public UserAPIContext(DbContextOptions<UserAPIContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
