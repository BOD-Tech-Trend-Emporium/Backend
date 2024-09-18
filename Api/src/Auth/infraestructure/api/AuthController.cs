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
using Microsoft.AspNetCore.Mvc;

namespace Api.src.Auth.infraestructure.api
{
    [Route("api")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private AuthRepository _authService;
        private AuthValidations _authValidations;
        
        public AuthController(AuthRepository authRepository, AuthValidations authValidations)
        {
            _authService = authRepository;
            _authValidations = authValidations;
        }

        [HttpPost("admin/auth")]
        public async Task<IActionResult> SignUpUser([FromBody] CreateUserDto user)
        {
            var userModel = user.ToUserModelForCreate();
            bool isEmailValid = _authValidations.IsEmailValid(userModel.Email);
            bool isEmailUnique = _authValidations.EmailExists(userModel);
            bool isRoleValid = _authValidations.IsRoleValid(userModel.Role);
            bool isUserNameUnique = _authValidations.UsernameExists(userModel);

            Console.WriteLine(userModel);
            Console.WriteLine(isEmailUnique);
            Console.WriteLine(isUserNameUnique);

            if(!isEmailValid){
                return BadRequest("Email not valid");
            }

            if(!isEmailUnique)
            {
                return Conflict("A user with this email alreay exists");
            }

            if(!isRoleValid)
            {
                return BadRequest("Role not valid");
            }

            if(!isUserNameUnique)
            {
                return Conflict("A user with this user name alreay exists");
            }
            
            await _authService.SignUpUser(userModel);
            return Created($"/api/user/{userModel.Id}", userModel.ToUserDto());
        }

        
    }
}