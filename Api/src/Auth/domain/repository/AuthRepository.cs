using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.src.User.domain.entity;

namespace Api.src.Auth.domain.repository
{
    public interface AuthRepository
    {
        Task<UserEntity?> SignUpUser(UserEntity user);
    }
}