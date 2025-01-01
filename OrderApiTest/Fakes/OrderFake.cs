using OrderApi.Entities;
using Org.OpenAPITools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OrderFake
{
    public OrderEntity expectedOrderEntity { get; set; }
    public Order expectedOrder { get; set; }
    public OrderFake(int orderId, int userId)
    {
        expectedOrderEntity = new OrderEntity
        {
            OrderId = orderId,
            UserId = userId,
            ProductIds = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Id = 1,
                    productId = 1
                }
            }
        };
        expectedOrder = new Order
        {
            OrderId = orderId,
            User = new User
            {
                UserId = userId,
                Name = $"User {userId}"
            },
            Products = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Product 1"
                }
            }
        };
    }
}
