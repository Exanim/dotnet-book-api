using Microsoft.EntityFrameworkCore;
using OrderApi.DbContexts;
using OrderApi.Entities;

namespace OrderApi.Services
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IOrdersContext _context;
        public OrdersRepository(IOrdersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void AddOrder(OrderEntity orderEntity)
        {
            _context.Orders.Add(orderEntity);
        }

        public async Task<IEnumerable<OrderEntity>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o=>o.ProductIds).ToListAsync();
        }

        public async Task<OrderEntity?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Where(o=>o.OrderId == id).Include(o=>o.ProductIds).FirstOrDefaultAsync();
        }

        public bool Savechanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void DeleteOrder(OrderEntity orderEntity)
        {
            // _context.Products.Remove(productEntity);
            _context.Orders.Remove(orderEntity);
        }
    }
}