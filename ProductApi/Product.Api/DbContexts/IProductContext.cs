using Microsoft.EntityFrameworkCore;
using Products.Api.Entities;

namespace Products.Api.DbContexts
{
    public interface IProductContext
    {
        DbSet<Product> Products { get; set; }
        Task<int> SaveChangesAsync();
    }
}