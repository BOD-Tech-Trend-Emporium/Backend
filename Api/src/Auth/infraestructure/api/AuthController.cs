using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.mappers;
using Api.src.Auth.application.validations;
using Api.src.Auth.domain.dto;
using Api.src.Auth.domain.repository;
using backend.src.User.application.mappers;
using backend.src.User.domain.dto;
using backend.src.User.domain.repository;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;

namespace Api.src.Auth.infraestructure.api
{
    [Route("api")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private AuthRepository _authService;
        private UserRepository _userService;
        private AuthValidations _authValidations;
        
        public AuthController(AuthRepository authRepository, UserRepository userRepository, AuthValidations authValidations)
        {
            _authService = authRepository;
            _userService = userRepository;
            _authValidations = authValidations;
        }

        [HttpPost("admin/auth")]
        public async Task<IActionResult> SignUpUser([FromBody] CreateUserDto user)
        {
            var userModel = user.ToUserModelForCreate();
            bool isEmailValid = _authValidations.IsEmailValid(userModel.Email);
            bool emailExists = _authValidations.EmailExists(userModel.Email);
            bool isRoleValid = _authValidations.IsRoleValid(userModel.Role);
            bool UserNameExists = _authValidations.UserNameExists(userModel.UserName);

            if(!isEmailValid){
                return BadRequest("Email not valid");
            }

            if(emailExists)
            {
                return Conflict("A user with this email alreay exists");
            }

            if(!isRoleValid)
            {
                return BadRequest("Role not valid");
            }

            if(UserNameExists)
            {
                return Conflict("A user with this user name alreay exists");
            }
            
            await _authService.SignUpUser(userModel);
            return Created($"/api/user/{userModel.Id}", userModel.ToUserDto());
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto user)
        {
            var userModel = await _userService.GetByEmailAsync(user.Email);
            var passwordsMatch = false;
            if(userModel != null)
            {
                passwordsMatch = BCrypt.Net.BCrypt.Verify(user.Password, userModel.Password);
            }

            if(userModel == null || !passwordsMatch)
            {
                return BadRequest("Wrong email or password");
            } 

            return Ok(userModel);
        }
    }
}