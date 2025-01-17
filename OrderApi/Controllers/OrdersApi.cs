/*
 * Orders
 *
 * API for Orders
 *
 * The version of the OpenAPI document: 0.0.1
 * 
 * Generated by: https://openapi-generator.tech
 */

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using Org.OpenAPITools.Attributes;
using Org.OpenAPITools.Models;

namespace Org.OpenAPITools.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public abstract class OrdersApi : ControllerBase
    {

        /// <summary>
        /// create an order
        /// </summary>
        /// <remarks>Associates product(s) with a user, creating an order</remarks>
        /// <param name="postBody"></param>
        /// <response code="204">no content</response>
        /// <response code="400">invalid post body</response>
        [HttpPost]
        [Route("/orders")]
        [ValidateModelState]
        [SwaggerOperation("AddOrder")]
        public abstract Task<IActionResult> AddOrder([FromBody] OrderPostBody postBody);

        /// <summary>
        /// get details of a specified order by ID
        /// </summary>
        /// <remarks>Returns details of an order</remarks>
        /// <param name="id">Integer ID of a user</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid id supplied</response>
        /// <response code="404">Order not found</response>
        [HttpGet]
        [Route("/orders/{id}")]
        [ValidateModelState]
        [SwaggerOperation("GetOrder")]
        [SwaggerResponse(statusCode: 200, type: typeof(Order), description: "successful operation")]
        public abstract Task<IActionResult> GetOrder([FromRoute(Name = "id")][Required] int id);

        /// <summary>
        /// get details of all orders
        /// </summary>
        /// <remarks>Returns an JSON object of an array of orders</remarks>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/orders")]
        [ValidateModelState]
        [SwaggerOperation("GetOrders")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Order>), description: "successful operation")]
        public abstract Task<IActionResult> GetOrders();

        /// <summary>
        /// delete a specified order by id
        /// </summary>
        /// <remarks>Deletes an order</remarks>
        /// <param name="id">Integer ID of an order</param>
        /// <response code="204">no content</response>
        /// <response code="400">Invalid id supplied</response>
        /// <response code="404">Order not found</response>
        [HttpDelete]
        [Route("/orders/{id}")]
        [ValidateModelState]
        [SwaggerOperation("RemoveOrder")]
        public abstract Task<IActionResult> RemoveOrder([FromRoute(Name = "id")][Required] int id);

        /// <summary>
        /// update a specified order by id
        /// </summary>
        /// <remarks>updates an order, can update multiple products in one order</remarks>
        /// <param name="id">Integer ID of an order</param>
        /// <param name="putBody"></param>
        /// <response code="204">no content</response>
        /// <response code="400">Invalid id supplied</response>
        /// <response code="404">Order not found</response>
        [HttpPut]
        [Route("/orders/{id}")]
        [ValidateModelState]
        [SwaggerOperation("UpdateOrder")]
        public abstract Task<IActionResult> UpdateOrder([FromRoute(Name = "id")][Required] int id, [FromBody] OrderPutBody putBody);
    }
}
