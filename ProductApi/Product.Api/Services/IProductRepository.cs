using Products.Api.Models;
using Products.Api.Entities;
using System.Collections.Generic;

namespace Products.Api.Services
{
    public interface IProductRepository
    {
        Task<Product?> GetProduct(int productId);
        Task<bool> ProductExists(int productId);
        void AddProduct(Product product);
        bool DeleteProduct(int productId);
        Task<IEnumerable<Product>> GetProducts();
        Task<bool> SavechangesAsync();
        }
}
