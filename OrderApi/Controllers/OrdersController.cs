using Microsoft.AspNetCore.Mvc;
using OrderApi.Services;
using Org.OpenAPITools.Controllers;
using Org.OpenAPITools.Models;
using System.Net;
using OrderApi.Entities;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using AutoMapper;
using OrderApi.Exceptions;
using OrderApi.Error;
using OrderApi.Migrations;

namespace OrderApi.Controllers
{
    public class OrdersController : OrdersApi
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrdersClients _ordersClients;
        private readonly IOrderMapper _orderMapper;

        public OrdersController(
            IOrdersRepository ordersRepository,
            IOrdersClients ordersClients,
            IOrderMapper orderMapper)
        {
            _ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
            _ordersClients = ordersClients ?? throw new ArgumentNullException(nameof(ordersClients));
            _orderMapper = orderMapper ?? throw new ArgumentNullException(nameof(orderMapper));
        }

        public override async Task<IActionResult> AddOrder([FromBody] OrderPostBody postBody)
        {
            using HttpResponseMessage response = await _ordersClients.GetUserAsync(postBody.UserId);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new OrderException(ErrorCode.UserNotFound, "The user you are looking for is not found in the database.");
            OrderEntity orderEntity = new OrderEntity();
            response.EnsureSuccessStatusCode();


            orderEntity.UserId = postBody.UserId;
            orderEntity.ProductIds = new List<ProductEntity>();
            foreach (int productId in postBody.ProductIds)
            {
                using HttpResponseMessage productResponse = await _ordersClients.GetProductAsync(productId);
                if (productResponse.StatusCode == HttpStatusCode.NotFound) 
                    throw new OrderException(ErrorCode.ProductNotFound, "The product you are looking for is not found in the database.");
                productResponse.EnsureSuccessStatusCode();
                var productEntity = new ProductEntity();
                productEntity.productId = productId;
                orderEntity.ProductIds.Add(productEntity);
            }
            _ordersRepository.AddOrder(orderEntity);
            _ordersRepository.Savechanges();
            return Ok();
        }
        public override async Task<IActionResult> GetOrder([FromRoute(Name = "id")][Required, Range(0,int.MaxValue)] int id) 
        {
            var order = await _ordersRepository.GetOrderByIdAsync(id);
            if (order == null) 
                throw new OrderException(ErrorCode.OrderNotFound, "The order you are looking for is not found in the database.");

            return Ok(await _orderMapper.ToOrderDTO(order));

        }

        public override async Task<IActionResult> GetOrders()
        {
            var orders = await _ordersRepository.GetAllOrdersAsync();
            List<Order> ordersList = new();
            foreach (var order in orders)
            {
                ordersList.Add(await _orderMapper.ToOrderDTO(order));
            }
            return Ok(ordersList);
        }

        public async override Task<IActionResult> RemoveOrder([FromRoute(Name = "id")][Required] int id)
        {

            var order = await _ordersRepository.GetOrderByIdAsync(id);
            if (order == null)
                throw new OrderException(ErrorCode.OrderNotFound, "The order you are looking for is not found in the database.");

            _ordersRepository.DeleteOrder(order);
            _ordersRepository.Savechanges();

            return NoContent();
        }

        public override async Task<IActionResult> UpdateOrder([FromRoute(Name = "id")][Required] int id, [FromBody] OrderPutBody putBody)
        {

            var order = await _ordersRepository.GetOrderByIdAsync(id);
            if (order == null)
                throw new OrderException(ErrorCode.OrderNotFound, "The order you are looking for is not found in the database.");

            using HttpResponseMessage response = await _ordersClients.GetUserAsync(putBody.UserId);
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new OrderException(ErrorCode.UserNotFound, "The user you are looking for is not found in the database.");
            order.UserId = putBody.UserId;
            order.ProductIds.Clear();

            foreach (var item in putBody.ProductIds)
            {
                using HttpResponseMessage productResponse = await _ordersClients.GetProductAsync(item);
                if (productResponse.StatusCode == HttpStatusCode.NotFound)
                    throw new OrderException(ErrorCode.ProductNotFound, "The product you are looking for is not found in the database.");
                order.ProductIds.Add(new ProductEntity { productId = item });
            }
            _ordersRepository.Savechanges();

            return NoContent();
        }
    }
}
