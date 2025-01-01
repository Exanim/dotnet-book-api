using Microsoft.EntityFrameworkCore;
using OrderApi.Entities;

namespace OrderApi.DbContexts
{
    public class OrdersContext : DbContext, IOrdersContext
    {
        public DbSet<OrderEntity> Orders { get; set; } = null!;
        public DbSet<ProductEntity> Products { get; set; }

        public OrdersContext(DbContextOptions<OrdersContext> options)
            : base(options) { }
        public int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
