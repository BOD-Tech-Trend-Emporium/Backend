using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.mappers;
using Api.src.Auth.application.Utils;
using Api.src.Auth.application.validations;
using Api.src.Auth.domain.dto;
using Api.src.Auth.domain.repository;
using Api.src.User.domain.dto;
using backend.src.User.application.mappers;
using backend.src.User.domain.dto;
using backend.src.User.domain.enums;
using backend.src.User.domain.repository;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.src.Auth.infraestructure.api
{
    [Route("api")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private AuthRepository _authService;
        
        public AuthController(AuthRepository authRepository)
        {
            _authService = authRepository;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> SignUpShopper([FromBody] CreateShopperDto user)
        {
            var userModel = user.ToUserModelForCreateShopper();
            await _authService.SignUpUser(userModel);
            return Created($"/api/user/{userModel.Id}", userModel.ToUserDto());
        }

        [HttpPost("admin/auth")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> SignUpUser([FromBody] CreateUserDto user)
        {   
            var userModel = user.ToUserModelForCreate();
            await _authService.SignUpShopper(userModel);
            return Created($"/api/user/{userModel.Id}", userModel.ToUserDto());
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto user)
        {
            var userModel = await _authService.LoginUser(user);
            return Ok(userModel);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            TokenDto tokenPayload = Token.GetTokenPayload(Request);
            string userId = tokenPayload.UserId;
            var sessionModel = await _authService.LogoutUser(userId);
            return Ok(sessionModel);
        }
    }
}