using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OrderApi.Controllers;
using OrderApi.Entities;
using OrderApi.Exceptions;
using OrderApi.Services;

namespace OrdersControllerTests
{
    public class RemoveOrderTest
    {
        Mock<IOrdersRepository> _ordersRepository;
        Mock<IOrdersClients> _ordersClients;
        Mock<IOrderMapper> _ordersMapper;
        OrdersController _ordersController;

        [SetUp]
        public void SetUp()
        {
            _ordersRepository = new Mock<IOrdersRepository>();
            _ordersClients = new Mock<IOrdersClients>();
            _ordersMapper = new Mock<IOrderMapper>();
            _ordersController = new OrdersController(_ordersRepository.Object, _ordersClients.Object, _ordersMapper.Object);
        }

        [Test]
        public async Task SuccessfullyRemovedOrder()
        {
            //arrange
            var exampleRequest = new OrderEntity
            {
                OrderId = 1,
                UserId = 2,
                ProductIds = new List<ProductEntity>
                    {
                        new ProductEntity { productId = 3 },
                        new ProductEntity { productId = 4 }
                    }
            };

            _ordersRepository.Setup(x => x.GetOrderByIdAsync(It.IsAny<int>())).ReturnsAsync(exampleRequest);

            //act
            var response = await _ordersController.RemoveOrder(exampleRequest.OrderId);

            //assert
            Assert.That(response, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task OrderNotFound()
        {
            //arrange
            var orderId = 1;

            _ordersRepository.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync((OrderEntity)null);

            //act & assert
            Assert.ThrowsAsync<OrderException>(async () => await _ordersController.RemoveOrder(orderId));
        }

    }
}
