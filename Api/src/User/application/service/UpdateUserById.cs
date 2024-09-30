using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.validations;
using Api.src.Common.exceptions;
using Api.src.User.domain.dto;
using backend.Data;
using backend.src.User.application.mappers;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;

namespace backend.src.User.application.service
{
    public class UpdateUserById
    {
        private readonly ApplicationDBContext _context;
        private GetUserById getUserByIdService;
        private AuthValidations authValidations;



        public UpdateUserById(ApplicationDBContext context)
        {
            _context = context;
            getUserByIdService = new GetUserById(context);
            authValidations = new AuthValidations(context);
        }
        public async Task<UpdateUserResultDto> Run(Guid id, UpdateUserDto user)
        {
            var userModel = await getUserByIdService.Run(id);

            if (userModel == null)
            {
                throw new NotFoundException($"User with id {id} not found");
            }

            if (!string.IsNullOrEmpty(user.Name))
            {
                userModel.Name = user.Name;
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                bool isEmailValid = authValidations.IsEmailValid(user.Email);

                if(!isEmailValid){
                    throw new BadRequestException("Email not valid");
                }

                bool emailExists = false;
            
                if(userModel.Email != user.Email)
                {
                    emailExists = await authValidations.EmailExists(user.Email);
                }

                if(emailExists)
                {
                    throw new ConflictException("A user with this email alreay exists");
                }

                userModel.Email = user.Email;
            }

            if (!string.IsNullOrEmpty(user.UserName))
            {
                bool UserNameExists = false;
            
                if(userModel.UserName != user.UserName)
                {
                    UserNameExists = await authValidations.UserNameExists(user.UserName);
                }

                if(UserNameExists)
                {
                    throw new ConflictException("A user with this username alreay exists");
                }

                userModel.UserName = user.UserName;
            }
            
            if (!string.IsNullOrEmpty(user.Password))
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

                userModel.Password = passwordHash;
            }

            await _context.SaveChangesAsync();

            return userModel.ToUserUpdateResultFromModel();
        }
    }
}