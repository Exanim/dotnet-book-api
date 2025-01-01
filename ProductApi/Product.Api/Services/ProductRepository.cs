using Products.Api.DbContexts;
using Products.Api.Models;
using Products.Api.Entities;
using SQLitePCL;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Products.Api.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductContext _context;
        public ProductRepository(IProductContext context) 
        { 
            _context = context ?? throw new ArgumentNullException(nameof(context)); 
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public bool DeleteProduct(int productId)
        {
            var productToDelete = _context.Products.Where(p => p.Id == productId).FirstOrDefault(); // assumes there is only one hit
            if (productToDelete == null) { return false; }
            _context.Products.Remove(productToDelete);
            return true;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProduct(int productId)
        {
            return await _context.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<bool> ProductExists(int productId)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId);
        }

        
        public async Task<bool> SavechangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
