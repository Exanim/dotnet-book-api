/*
 * Users
 *
 * Basic CRUD api for Users
 *
 * The version of the OpenAPI document: 0.0.1
 * 
 * Generated by: https://openapi-generator.tech
 */

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.OpenAPITools.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using userAPI.DTOs;
using userAPI.Entities;

namespace Org.OpenAPITools.Controllers
{
    /// <summary>
    /// User API
    /// </summary>
    [ApiController]
    public abstract class UserApi : ControllerBase
    {
        /// <summary>
        /// create a user
        /// </summary>
        /// <remarks>Creates a user</remarks>
        /// <param name="postBody"></param>
        /// <response code="204">no content</response>
        /// <response code="400">invalid post body</response>
        [HttpPost]
        [Route("/user")]
        [ValidateModelState]
        [SwaggerOperation("AddUser")]
        public abstract Task<IActionResult> AddUser([FromBody] UserCreationDto postBody);

        /// <summary>
        /// get a specified user
        /// </summary>
        /// <remarks>Returns a user</remarks>
        /// <param name="id">Integer ID of a user</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid id supplied</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/user/{id}")]
        [ValidateModelState]
        [SwaggerOperation("GetUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(User), description: "successful operation")]
        public abstract Task<IActionResult> GetUser([FromRoute (Name = "id")][Required]int id);

        /// <summary>
        /// get all users
        /// </summary>
        /// <remarks>Returns an Array of users</remarks>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/users")]
        [ValidateModelState]
        [SwaggerOperation("GetUsers")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<User>), description: "successful operation")]
        public abstract Task<IActionResult> GetUsers();

        /// <summary>
        /// partially update a specified user
        /// </summary>
        /// <remarks>partially updates a user using JsonPatch</remarks>
        /// <param name="id">Integer ID of a user</param>
        /// <param name="patchBody">JsonPatch</param>
        /// <response code="204">no content</response>
        /// <response code="400">Invalid id supplied</response>
        /// <response code="404">User not found</response>
        [HttpPatch]
        [Route("/user/{id}")]
        [ValidateModelState]
        [SwaggerOperation("PartiallyUpdateUser")]
        public abstract Task<IActionResult> PartiallyUpdateUser([FromRoute(Name = "id")][Required] int id, [FromBody] JsonPatchDocument<UserDto> patchBody);
        /// <summary>
        /// delete a specified user
        /// </summary>
        /// <remarks>Deletes a user</remarks>
        /// <param name="id">Integer ID of a user</param>
        /// <response code="204">no content</response>
        /// <response code="400">Invalid id supplied</response>
        /// <response code="404">User not found</response>
        [HttpDelete]
        [Route("/user/{id}")]
        [ValidateModelState]
        [SwaggerOperation("RemoveUser")]
        public abstract Task<IActionResult> RemoveUser([FromRoute(Name = "id")][Required] int id);
    }
}
