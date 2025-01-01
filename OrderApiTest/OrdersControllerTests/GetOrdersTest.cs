using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderApi.Controllers;
using OrderApi.Entities;
using OrderApi.Services;
using Org.OpenAPITools.Models;
using NUnit.Framework;

namespace OrdersControllerTests
{
    public class GetOrdersTest
    {
        Mock<IOrdersRepository> _ordersRepository;
        Mock<IOrderMapper> _orderMapper;
        OrdersController _ordersController;
        List<OrderEntity> _sampleOrders;
        List<Order> _expectedOrderDTOs;

        [SetUp]
        public void SetUp()
        {
            _orderMapper = new Mock<IOrderMapper>();
            _ordersRepository = new Mock<IOrdersRepository>();
            _ordersController = new OrdersController(_ordersRepository.Object,Mock.Of<IOrdersClients>(), _orderMapper.Object);
            _sampleOrders = new List<OrderEntity>
            {
                new OrderFake(1,1).expectedOrderEntity,
                new OrderFake(2,2).expectedOrderEntity,
                new OrderFake(3,3).expectedOrderEntity,
                new OrderFake(4,4).expectedOrderEntity,
            };
            _expectedOrderDTOs = new List<Order>
            {
                new OrderFake(1,1).expectedOrder,
                new OrderFake(2,2).expectedOrder,
                new OrderFake(3,3).expectedOrder,
                new OrderFake(4,4).expectedOrder
            };
        }

        [Test]
        public async Task GetOrders_Returns_OkResult()
        {

            _ordersRepository.Setup(repo => repo.GetAllOrdersAsync())
                .ReturnsAsync(_sampleOrders);

            _orderMapper.Setup(mapper => mapper.ToOrderDTO(It.IsAny<OrderEntity>()))
            .Returns<OrderEntity>((orderEntity) =>
            {
                var expectedOrderDto = _expectedOrderDTOs.FirstOrDefault(dto => dto.OrderId == orderEntity.OrderId);
                return Task.FromResult(expectedOrderDto)!;
            });

            // Act
            var result = await _ordersController.GetOrders();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

        }
        [Test]
        public async Task GetOrders_Returns_ObjectDTOList()
        {

            _ordersRepository.Setup(repo => repo.GetAllOrdersAsync())
                .ReturnsAsync(_sampleOrders);

            _orderMapper.Setup(mapper => mapper.ToOrderDTO(It.IsAny<OrderEntity>()))
            .Returns<OrderEntity>((orderEntity) =>
            {
                var expectedOrderDto = _expectedOrderDTOs.FirstOrDefault(dto => dto.OrderId == orderEntity.OrderId);
                return Task.FromResult(expectedOrderDto)!;
            });

            // Act
            var result = await _ordersController.GetOrders();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = result as OkObjectResult;

            Assert.That(okResult?.Value, Is.TypeOf<List<Order>>());

            var orderlist = okResult?.Value as List<Order>;

            Assert.That(_sampleOrders.Count, Is.EqualTo(orderlist.Count));

            foreach (var order in orderlist)
            {
                Assert.That(order, Is.Not.Null);
            }
        }
    }
}