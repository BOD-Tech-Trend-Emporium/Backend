using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Session.domain.entity;
using backend.src.User.domain.entity;

namespace Api.src.Session.application.mappers
{
    public static class SessionMappers
    {
        public static SessionEntity ToSessionModelForCreate(this UserEntity user)
        {
            return new SessionEntity{
                UserId = user.Id,
                IsActive = false
            };
        }
    }
}