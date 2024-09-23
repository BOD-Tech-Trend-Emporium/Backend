using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.validations;
using Api.src.Common.exceptions;
using Api.src.Session.application.mappers;
using Api.src.Session.application.service;
using backend.Data;
using backend.src.User.domain.entity;

namespace Api.src.Auth.application.service
{
    public class SignUpShopper
    {
        private readonly ApplicationDBContext _context;
        private AuthValidations authValidations;
        private CreateSession createSessionService;

        public SignUpShopper(ApplicationDBContext context)
        {
            _context = context;
            authValidations = new AuthValidations(context);
            createSessionService = new CreateSession(context);
        }

        public async Task<UserEntity?> Run(UserEntity user)
        {
            bool isEmailValid = authValidations.IsEmailValid(user.Email);
            bool emailExists = await authValidations.EmailExists(user.Email);
            bool UserNameExists = await authValidations.UserNameExists(user.UserName);

            if(!isEmailValid){
                throw new BadRequestException("Email not valid");
            }

            if(emailExists)
            {
                throw new ConflictException("A user with this email alreay exists");
            }

            if(UserNameExists)
            {
                throw new ConflictException("A user with this username alreay exists");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passwordHash;
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            var sessionModel = user.ToSessionModelForCreate();
            await createSessionService.Run(sessionModel);  

            return user;
        }
    }
}