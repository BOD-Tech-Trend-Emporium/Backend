using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.validations;
using Api.src.Common.exceptions;
using Azure.Core;
using backend.Data;
using backend.src.User.domain.entity;
using BCrypt.Net;

namespace Api.src.Auth.application.service
{
    public class SignUpUser
    {
        private readonly ApplicationDBContext _context;
        private AuthValidations authValidations;

        public SignUpUser(ApplicationDBContext context)
        {
            _context = context;
            authValidations = new AuthValidations(context);
        }

        public async Task<UserEntity?> Run(UserEntity user)
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

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passwordHash;
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}