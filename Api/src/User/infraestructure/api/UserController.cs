using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.src.User.application.mappers;
using backend.src.User.application.service;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;
using backend.src.User.domain.repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace backend.src.User.infraestructure.api
{
    [Route("api/user")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private UserRepository _userService;

        
        public UserController(UserRepository userRepository)
        {
            _userService = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            var usersList = users.Select(i => i.ToUserDto());
            return Ok(usersList);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if(user == null)
            {
                return NotFound($"User with id {id} not found");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto user)
        {
            var userModel = user.ToUserModelForCreate();
            await _userService.CreateAsync(userModel);
            return Created($"/api/user/{userModel.Id}", userModel.ToUserDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var user = await _userService.DeleteByIdAsync(id);

            if(user == null)
            {
                return NotFound($"User with id {id} not found");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserDto user)
        {
            var userModel = await _userService.UpdateByIdAsync(id, user);

            if(userModel == null)
            {
                return NotFound($"User with id {id} not found");
            }

            return Ok(userModel);
        }
    }
}