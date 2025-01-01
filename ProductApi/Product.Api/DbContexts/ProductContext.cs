using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Products.Api.Entities;
using Products.Api.Models;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Products.Api.DbContexts
{

    public class ProductContext : DbContext, IProductContext
    {
        public virtual DbSet<Product> Products { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            CancellationToken cancellationToken = CancellationToken.None;
            return await base.SaveChangesAsync(cancellationToken);
        }

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }
    }
}
