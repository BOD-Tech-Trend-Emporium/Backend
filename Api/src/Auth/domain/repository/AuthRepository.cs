using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.domain.dto;
using backend.src.User.domain.entity;

namespace Api.src.Auth.domain.repository
{
    public interface AuthRepository
    {
        Task<UserEntity?> SignUpUser(UserEntity user);

        Task<LoggedUserDto?> LoginUser(UserLoginDto user);
    }
}