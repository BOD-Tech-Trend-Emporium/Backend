using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.domain.dto;
using Api.src.Session.domain.entity;
using backend.src.User.domain.entity;

namespace Api.src.Auth.domain.repository
{
    public interface AuthRepository
    {
        Task<UserEntity?> SignUpUser(UserEntity user);
        Task<UserEntity?> SignUpShopper(UserEntity user);
        Task<LoggedUserDto?> LoginUser(UserLoginDto user);
        Task<SessionEntity?> LogoutUser(string id);
    }
}