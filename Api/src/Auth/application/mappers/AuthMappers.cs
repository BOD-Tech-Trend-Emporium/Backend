using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.domain.dto;
using backend.src.User.domain.entity;

namespace Api.src.Auth.application.mappers
{
    public static class AuthMappers
    {
        public static LoggedUserDto ToLoggedUser(UserEntity user, string token)
        {
            return new LoggedUserDto
            {
                Token = token,
                Email = user.Email,
                UserName = user.UserName
            };
        }
    }
}