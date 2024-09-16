using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;

namespace backend.src.User.application.mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this UserEntity user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserName = user.Name,
                Role = user.Role,
                Status = user.Status,
    };
        }

        public static UserEntity ToUserModelForCreate(this CreateUserDto user)
        {
            return new UserEntity
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.Name,
                Password = user.Password,
                Role = user.Role,
                Status = user.Status,
            };
        }

        public static UserEntity ToUserModelForUpdate (this UpdateUserDto user)
        {
            return new UserEntity
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.Name,
                Role = user.Role,
                Status = user.Status,
            };
        }
    }
}