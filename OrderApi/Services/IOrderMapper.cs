using OrderApi.Entities;
using Org.OpenAPITools.Models;

namespace OrderApi.Services
{
    public interface IOrderMapper
    {
        Task<Order> ToOrderDTO(OrderEntity order);
    }
}
