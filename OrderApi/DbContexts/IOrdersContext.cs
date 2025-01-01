using Microsoft.EntityFrameworkCore;
using OrderApi.Entities;

namespace OrderApi.DbContexts
{
    public interface IOrdersContext
    {
        DbSet<OrderEntity> Orders { get; set; }
        DbSet<ProductEntity> Products { get; set; }
        int SaveChanges();
    }
}