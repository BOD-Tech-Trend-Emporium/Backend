using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.src.Auth.application.Utils;
using Api.src.Auth.domain.dto;
using backend.Data;
using backend.src.User.application.mappers;
using backend.src.User.application.service;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using backend.src.User.domain.repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = nameof(UserRole.Admin))]
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
        [Route("delete/self")]
        [ProducesResponseType(204)]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            var user = await _userService.DeleteByIdAsync(Guid.Parse(Token.GetTokenPayload(Request).UserId));
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserDto user)
        {
            var updatedUser = await _userService.UpdateByIdAsync(id, user);
            return Ok(updatedUser);
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> DeleteUsers([FromBody] List<string> userEmails)
        {
            await _userService.DeleteUsers(userEmails);
            return StatusCode(204);
        }
    }
}