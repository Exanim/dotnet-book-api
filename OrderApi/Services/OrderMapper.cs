using Newtonsoft.Json;
using OrderApi.Entities;
using OrderApi.Error;
using OrderApi.Exceptions;
using OrderApi.Models;
using Org.OpenAPITools.Models;
using System.Security.AccessControl;

namespace OrderApi.Services
{
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="NullReferenceException"/>
    /// <exception cref="ProductNotFoundException"/>
    /// <exception cref="UserNotFoundException"/>
    public class OrderMapper : IOrderMapper
    {
        private readonly ICache _cache;
        private readonly IOrdersClients _ordersClients;

        public OrderMapper(ICache cache, IOrdersClients ordersClients)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _ordersClients = ordersClients ?? throw new ArgumentNullException(nameof(ordersClients));
        }
        public async Task<Order> ToOrderDTO(OrderEntity order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            User user;
            string usertag = "";

            if (_cache.IsCached(CacheType.User, order.UserId))
            {
                user = (User)_cache.Get(CacheType.User, order.UserId)!;
                usertag = " - from cache";
            }else{
                var userResponse = await _ordersClients.GetUserAsync(order.UserId);

                if (!userResponse.IsSuccessStatusCode) throw new OrderException(ErrorCode.UserNotFound, "The user you are looking for is not found in the database.");
                var userJson = userResponse.Content.ReadAsStringAsync().Result ?? throw new NullReferenceException();

                user = JsonConvert.DeserializeObject<User>(userJson)!;

                _cache.Add(CacheType.User, order.UserId, user);
            }

            List<Product> prods = new();

            foreach (var prod in order.ProductIds)
            {
                ProductGet product;
                string prodtag = "";

                if (_cache.IsCached(CacheType.Product, prod.productId))
                {
                    product = (ProductGet)_cache.Get(CacheType.Product, prod.productId)!;
                    prodtag = " - from cache";
                }
                else
                {
                    var prodResponse = await _ordersClients.GetProductAsync(prod.productId);
                    if (!prodResponse.IsSuccessStatusCode) throw new OrderException(ErrorCode.ProductNotFound, "The product you are looking for is not found in the database.");

                    var prodJson = prodResponse.Content.ReadAsStringAsync().Result ?? throw new NullReferenceException();
                    product = JsonConvert.DeserializeObject<ProductGet>(prodJson)!;

                    _cache.Add(CacheType.Product,prod.productId, product);
                }

                prods.Add(new Product
                {
                    ProductId = prod.productId,
                    Name = product.Product.ProductName + prodtag
                });
            }

            return new Order()
            {
                OrderId = order.OrderId,
                User = new User()
                {
                    UserId = order.UserId,
                    Name = user.Name + usertag
                },
                Products = prods
            };
        }
    }
}
