using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OrderApi.Controllers;
using OrderApi.Entities;
using OrderApi.Exceptions;
using OrderApi.Services;
using Org.OpenAPITools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrdersControllerTests
{
    public class AddOrderTest
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
        public async Task SuccessfullyAddedOrder() 
        {
            //arrange
            var postBody = new OrderPostBody { UserId = 2, ProductIds = new List<int> { 3, 4 } };

            _ordersClients.Setup(x => x.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            _ordersClients.Setup(x => x.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            //act
            var response = await _ordersController.AddOrder(postBody);

            //assert
            Assert.That(response, Is.InstanceOf<OkResult>());
        }


        [Test]
        public async Task UserNotFound()
        {
            //arrange
            var postBody = new OrderPostBody { UserId = 2, ProductIds = new List<int> { 3, 4 } };

            _ordersClients.Setup(x => x.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

            //act & assert
            Assert.ThrowsAsync<OrderException>(async () => await _ordersController.AddOrder(postBody));
        }

        [Test]
        public async Task ProductNotFound()
        {
            //arrange
            var postBody = new OrderPostBody { UserId = 2, ProductIds = new List<int> { 3, 4 } };

            _ordersClients.Setup(x => x.GetUserAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            _ordersClients.Setup(x => x.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

            //act & assert
            Assert.ThrowsAsync<OrderException>(async () => await _ordersController.AddOrder(postBody));
        }
    }
}
