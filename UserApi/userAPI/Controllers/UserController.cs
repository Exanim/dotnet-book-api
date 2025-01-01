using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using userAPI.DTOs;
using userAPI.Entities;
using userAPI.Repositories;

namespace userAPI.Controllers
{
    [ApiController]
    public class UserController : UserApi
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper) { 
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async override Task<IActionResult> AddUser([FromBody] UserCreationDto postBody)
        {
            var user = _mapper.Map<User>(postBody);
            _userRepository.AddUser(user);
            await _userRepository.SaveChangesAsync();
            return Ok(user);
        }
        public async override Task<IActionResult> GetUser([FromRoute(Name = "id"), Required] int id)
        {
            if(!await _userRepository.DoesUserExists(id))
            {
                return NotFound();
            }
            return Ok(await _userRepository.GetUserAsync(id));
        }
        public async override Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }
        public async override Task<IActionResult> PartiallyUpdateUser([FromRoute(Name = "id"), Required] int id, [FromBody] JsonPatchDocument<UserDto> patchBody)
        {
            var userEntity = await _userRepository.GetUserAsync(id);
            var userToPatch = _mapper.Map<UserDto>(userEntity);

            patchBody.ApplyTo(userToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (TryValidateModel(userToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(userToPatch, userEntity);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }

        public async override Task<IActionResult> RemoveUser([FromRoute(Name = "id"), Required] int id)
        {
            var user = await _userRepository.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _userRepository.DeleteUser(user);
            await _userRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
