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
using Api.src.Session.domain.entity;
using Api.src.User.application.service;
using backend.Data;
using backend.src.User.domain.entity;

namespace Api.src.Auth.infraestructure
{
    public class AuthService: AuthRepository
    {
        private SignUpUser signUpUserService;
        private LoginUser loginUserService;
        private LogoutUser logoutUserService;
        private SignUpShopper signUpShopperService;

        public AuthService(ApplicationDBContext context, IConfiguration configuration)
        {
            signUpUserService = new SignUpUser(context);
            loginUserService = new LoginUser(context, configuration);
            logoutUserService = new LogoutUser(context);
            signUpShopperService = new SignUpShopper(context);
        }

        public async Task<UserEntity?> SignUpUser(UserEntity user)
        {
            return await signUpUserService.Run(user);
        }

        public async Task<UserEntity?> SignUpShopper(UserEntity user)
        {
            return await signUpShopperService.Run(user);
        }
    
        public async Task<LoggedUserDto?> LoginUser(UserLoginDto user)
        {
            return await loginUserService.Run(user);
        }

        public async Task<SessionEntity?> LogoutUser(string id)
        {
            return await logoutUserService.Run(id);
        }
    }
}