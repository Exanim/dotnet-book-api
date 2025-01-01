using OrderApi.Entities;

namespace OrderApi.Services
{
    public interface IOrdersRepository
    {
        void AddOrder(OrderEntity orderEntity);
        Task<OrderEntity?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderEntity>> GetAllOrdersAsync();
        bool Savechanges();
        void DeleteOrder(OrderEntity order);
    }
}