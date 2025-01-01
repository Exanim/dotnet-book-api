using Moq;
using OrderApi.Controllers;
using OrderApi.Services;
using NUnit.Framework;
using OrderApi.Entities;
using Org.OpenAPITools.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Exceptions;

namespace OrdersControllerTests
{
    public class UpdateOrderTest
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
        public async Task SuccessfullyUpdatedOrder()
        {
            //arrange
            var putBody = new OrderPutBody { UserId = 3, ProductIds = new List<int> { 5, 6 } };
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
            _ordersClients.Setup(x => x.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            _ordersClients.Setup(x => x.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            //act
            var response = await _ordersController.UpdateOrder(exampleRequest.OrderId, putBody);

            //assert
            Assert.That(response, Is.InstanceOf<NoContentResult>());
            Assert.AreEqual(putBody.UserId, exampleRequest.UserId);
            CollectionAssert.AreEqual(putBody.ProductIds, exampleRequest.ProductIds.Select(p => p.productId).ToList());
        }


        [Test]
        public async Task OrderNotFound()
        {
            //arrange
            var orderId = 1;
            var putBody = new OrderPutBody { UserId = 2, ProductIds = new List<int> { 3, 4 } };

            _ordersRepository.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync((OrderEntity)null);

            //act & assert
            Assert.ThrowsAsync<OrderException>(async () => await _ordersController.UpdateOrder(orderId, putBody));
        }

        [Test]
        public async Task UserNotFound()
        {
            //arrange
            var orderId = 1;
            var putBody = new OrderPutBody { UserId = 2, ProductIds = new List<int> { 3, 4 } };

            _ordersRepository.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(new OrderEntity());
            _ordersClients.Setup(x => x.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));


            //act & assert
            Assert.ThrowsAsync<OrderException>(async () => await _ordersController.UpdateOrder(orderId, putBody));
        }

        [Test]
        public async Task ProductNotFound()
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
            var putBody = new OrderPutBody { UserId = 3, ProductIds = new List<int> { 5, 6 } };

            _ordersRepository.Setup(x => x.GetOrderByIdAsync(It.IsAny<int>())).ReturnsAsync(exampleRequest);
            _ordersClients.Setup(x => x.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            _ordersClients.Setup(x => x.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));


            //act & assert
            Assert.ThrowsAsync<OrderException>(async () => await _ordersController.UpdateOrder(exampleRequest.OrderId, putBody));
        }
    }
}
