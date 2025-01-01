using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderApi.Services;
using NUnit.Framework;
using Org.OpenAPITools.Models;
using OrderApi.Controllers;

namespace OrdersControllerTests
{
    public class GetOrderTest
    {
        Mock<IOrdersRepository> _ordersRepository;
        Mock<IOrderMapper> _orderMapper;
        OrdersController _ordersController;
        OrderFake _orderFake;
        int _orderId;

        [SetUp]
        public void SetUp() { 
            _ordersRepository = new Mock<IOrdersRepository>();
            _orderMapper = new Mock<IOrderMapper>();
            _ordersController = new OrdersController( _ordersRepository.Object, Mock.Of<IOrdersClients>(), _orderMapper.Object);
            _orderId = 1;
            _orderFake = new OrderFake(_orderId, _orderId);
        }

        [Test]
        public async Task GetOrder_OrderFound_Returns_OkResult()
        {
            _ordersRepository.Setup(repo => repo.GetOrderByIdAsync(_orderId)).ReturnsAsync(_orderFake.expectedOrderEntity);

            _orderMapper.Setup(mapper => mapper.ToOrderDTO(_orderFake.expectedOrderEntity)).ReturnsAsync(_orderFake.expectedOrder);

            // Act
            var result = await _ordersController.GetOrder(_orderId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());


        }
        [Test]
        public async Task GetOrder_OrderFound_Returns_OrderDTO()
        {
            _ordersRepository.Setup(repo => repo.GetOrderByIdAsync(_orderId)).ReturnsAsync(_orderFake.expectedOrderEntity);

            _orderMapper.Setup(mapper => mapper.ToOrderDTO(_orderFake.expectedOrderEntity)).ReturnsAsync(_orderFake.expectedOrder);
            // Act
            var result = await _ordersController.GetOrder(_orderId);
            // Assert
            Assert.That(((OkObjectResult)result).Value, Is.TypeOf<Order>());
            Assert.That(((OkObjectResult)result).Value, Is.Not.Null);
        }
        [Test]
        public async Task GetOrder_OrderFound_Returns_Correct_DTO()
        {
            _ordersRepository.Setup(repo => repo.GetOrderByIdAsync(_orderId)).ReturnsAsync(_orderFake.expectedOrderEntity);

            _orderMapper.Setup(mapper => mapper.ToOrderDTO(_orderFake.expectedOrderEntity)).ReturnsAsync(_orderFake.expectedOrder);
            // Act
            var result = await _ordersController.GetOrder(_orderId);
            // Assert
            Assert.That(((OkObjectResult)result).Value, Is.EqualTo(_orderFake.expectedOrder));
        }


    }
}