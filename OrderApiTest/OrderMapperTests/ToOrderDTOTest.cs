using Moq;
using OrderApi.Services;
using Org.OpenAPITools.Models;
using System.Net;
using NUnit.Framework;

namespace OrderMapperTests
{
    public class ToOrderDTOTest
    {
        Mock<ICache> _cache;
        Mock<IOrdersClients> _ordersClients;
        OrderMapper _orderMapper;

        [SetUp]
        public void SetUp()
        {
            _cache = new Mock<ICache>();
            _ordersClients = new Mock<IOrdersClients>();
            _orderMapper = new OrderMapper(_cache.Object,_ordersClients.Object);
        }

        [Test]
        public async Task Returns_OrderDTO()
        {
            var orderEntity = new OrderFake(1, 1).expectedOrderEntity;
            var expectedOrder = new OrderFake(1, 1).expectedOrder;

            // not in cache
            _cache.Setup(c => c.IsCached(CacheType.User,orderEntity.UserId)).Returns(false);
            _cache.Setup(c => c.IsCached(CacheType.Product,orderEntity.ProductIds.First().productId)).Returns(false);
            _ordersClients.Setup(cl => cl.GetUserAsync(orderEntity.UserId)).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"Name\":\"" + expectedOrder.User.Name + "\"}")
            }
            );
            _ordersClients.Setup(client => client.GetProductAsync(It.IsAny<int>())).ReturnsAsync((int productId) =>
            {
                var prod = expectedOrder.Products.First(p => p.ProductId == productId);

                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"Product\":{\"ProductName\":\"" + prod.Name + "\"}}")
                };
            });

            var result = await _orderMapper.ToOrderDTO(orderEntity);

            Assert.That(result, Is.TypeOf<Order>());
        }
        [Test]
        public async Task Returns_Correct_DTO()
        {
            // Arrange
            var orderEntity = new OrderFake(1, 1).expectedOrderEntity;
            var expectedOrder = new OrderFake(1, 1).expectedOrder;

            // not in cache
            _cache.Setup(c => c.IsCached(CacheType.User, orderEntity.UserId)).Returns(false);
            _cache.Setup(c => c.IsCached(CacheType.Product, orderEntity.ProductIds.First().productId)).Returns(false);
            _ordersClients.Setup(cl => cl.GetUserAsync(orderEntity.UserId)).ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"Name\":\"" + expectedOrder.User.Name + "\"}")
                }
            );
            _ordersClients.Setup(client => client.GetProductAsync(It.IsAny<int>()))
                    .ReturnsAsync((int productId) =>
                    {
                        var prod = expectedOrder.Products.First(p => p.ProductId == productId);

                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent("{\"Product\":{\"ProductName\":\"" + prod.Name + "\"}}")
                        };
                    });

            // Act
            var result = await _orderMapper.ToOrderDTO(orderEntity);

            // Assert
            Assert.That(result, Is.EqualTo(expectedOrder));
        }
        [Test]
        public async Task Throws_ArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _orderMapper.ToOrderDTO(null));
        }
    }
}