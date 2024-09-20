using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.mappers;
using Api.src.Auth.application.service;
using Api.src.Auth.application.Utils;
using Api.src.Auth.application.validations;
using Api.src.Auth.domain.dto;
using Api.src.Auth.domain.repository;
using Api.src.Common.exceptions;
using Api.src.User.application.service;
using backend.Data;
using backend.src.User.domain.entity;

namespace Api.src.Auth.infraestructure
{
    public class AuthService: AuthRepository
    {
        private SignUpUser signUpUserService;
        private GetUserByEmail getUserByEmailService;
        private AuthValidations authValidations;
        private  IConfiguration _configuration;

        public AuthService(ApplicationDBContext context, IConfiguration configuration)
        {
            signUpUserService = new SignUpUser(context);
            getUserByEmailService = new GetUserByEmail(context);
            authValidations = new AuthValidations(context);
            _configuration = configuration;
        }

        public async Task<UserEntity?> SignUpUser(UserEntity user)
        {
            bool isEmailValid = authValidations.IsEmailValid(user.Email);
            bool emailExists = authValidations.EmailExists(user.Email);
            bool isRoleValid = authValidations.IsRoleValid(user.Role);
            bool UserNameExists = authValidations.UserNameExists(user.UserName);

            if(!isEmailValid){
                throw new BadRequestException("Email not valid");
            }

            if(emailExists)
            {
                throw new ConflictException("A user with this email alreay exists");
            }

            if(!isRoleValid)
            {
                throw new BadRequestException("Role not valid");
            }

            if(UserNameExists)
            {
                throw new ConflictException("A user with this username alreay exists");
            }

            return await signUpUserService.Run(user);
        }
    
        public async Task<LoggedUserDto?> LoginUser(UserLoginDto user)
        {
            var userModel = await getUserByEmailService.Run(user.Email);
            var passwordsMatch = false;

            if(userModel != null)
            {
                passwordsMatch = BCrypt.Net.BCrypt.Verify(user.Password, userModel.Password);
            }

            if(userModel == null || !passwordsMatch)
            {
                throw new BadRequestException("Wrong email or password");
            }

            string tokenKey = _configuration.GetSection("AppSettings:TokenKey").Value!;
            string token = Token.CreateToken(userModel, tokenKey);

            var loggedUser = AuthMappers.ToLoggedUser(userModel, token);

            return loggedUser;
        }
    }
}