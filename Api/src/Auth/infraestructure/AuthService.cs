using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.service;
using Api.src.Auth.domain.repository;
using backend.Data;
using backend.src.User.domain.entity;

namespace Api.src.Auth.infraestructure
{
    public class AuthService: AuthRepository
    {
        private SignUpUser signUpUserService;

        public AuthService(ApplicationDBContext context)
        {
            signUpUserService = new SignUpUser(context);
        }

        public async Task<UserEntity?> SignUpUser(UserEntity user)
        {   
            return await signUpUserService.Run(user);
        }
    }
}