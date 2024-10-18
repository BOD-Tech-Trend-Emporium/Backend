using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Common.exceptions;
using Api.src.User.domain.dto;
using Api.src.User.domain.enums;
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
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role,
                SecurityCuestion = user.SecurityQuestion.GetSecurityCuestionString(),
            };
        }

        public static UserEntity ToUserModelForCreate(this CreateUserDto user)
        {
            return new UserEntity
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role,
                Status = UserStatus.Created,
                SecurityQuestion = user.IntToSecurityQuestion(),
                SecurityQuestionAnswer = user.SecurityQuestionAnswer,
            };
        }

        public static UserEntity ToUserModelForCreateShopper(this CreateShopperDto user)
        {
            return new UserEntity
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                Password = user.Password,
                Role = UserRole.Shopper,
                Status = UserStatus.Created,
                SecurityQuestion = user.IntToSecurityQuestionShopper(),
                SecurityQuestionAnswer = user.SecurityQuestionAnswer,
            };
        }

        public static UpdateUserResultDto ToUserUpdateResultFromModel (this UserEntity user)
        {
            return new UpdateUserResultDto
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
            };
        }

        public static SecurityQuestion IntToSecurityQuestionShopper(this CreateShopperDto user)
        {
            switch (user.SecurityQuestion)
            {
                case 0:
                    return SecurityQuestion.securityQuestion0;
                case 1:
                    return SecurityQuestion.securityQuestion1;
                case 2:
                    return SecurityQuestion.securityQuestion2;
                case 3:
                    return SecurityQuestion.securityQuestion3;
                case 4:
                    return SecurityQuestion.securityQuestion4;
                default:
                    throw new NotFoundException($"Security Question with id {user.SecurityQuestion} do not exist");
            }
        }
        public static SecurityQuestion IntToSecurityQuestion(this CreateUserDto user)
        {
            switch (user.SecurityQuestion)
            {
                case 0:
                    return SecurityQuestion.securityQuestion0;
                case 1:
                    return SecurityQuestion.securityQuestion1;
                case 2:
                    return SecurityQuestion.securityQuestion2;
                case 3:
                    return SecurityQuestion.securityQuestion3;
                case 4:
                    return SecurityQuestion.securityQuestion4;
                default:
                    throw new NotFoundException($"Security Question with id {user.SecurityQuestion} do not exist");
            }
        }
        public static string GetSecurityCuestionString(this SecurityQuestion securityQuestion) 
        {
            switch (securityQuestion)
            {
                case SecurityQuestion.securityQuestion0:
                    return "What was the name of your first pet?";
                case SecurityQuestion.securityQuestion1:
                    return "What is the name of the street where you grew up?";
                case SecurityQuestion.securityQuestion2:
                    return "What was your childhood nickname?";
                case SecurityQuestion.securityQuestion3:
                    return "What is your mother’s maiden name?";
                case SecurityQuestion.securityQuestion4:
                    return "What was the model of your first car?";
                default:
                    throw new NotFoundException($"Security Question with id {securityQuestion} do not exist");
            }
        }
    }
}