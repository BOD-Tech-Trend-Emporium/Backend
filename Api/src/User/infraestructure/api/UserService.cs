using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.User.application.service;
using Api.src.User.domain.dto;
using backend.Data;
using backend.src.User.application.service;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;
using backend.src.User.domain.repository;
using Microsoft.EntityFrameworkCore;
using Api.src.User.domain.enums;
using backend.src.User.application.mappers;

namespace backend.src.User.infraestructure.api
{
    public class UserService: UserRepository
    {
        private GetAllUsers getAllUsersService;
        private CreateUser createUserService;
        private DeleteUserById deleteUserByIdService;
        private GetUserById getUserByIdService;
        private GetUserByEmail getUserByEmailService;
        private UpdateUserById updateUserByIdService;
        private DeleteUsers deleteUsers;

        public UserService(ApplicationDBContext context)
        {
            getAllUsersService = new GetAllUsers(context);
            createUserService = new CreateUser(context);
            deleteUserByIdService = new DeleteUserById(context);
            getUserByIdService = new GetUserById(context);
            getUserByEmailService = new GetUserByEmail(context);
            updateUserByIdService = new UpdateUserById(context);
            deleteUsers = new DeleteUsers(context);
        }

        public async Task<List<UserEntity>> GetAllAsync()
        {
            return await getAllUsersService.Run();
        }

        public async Task<UserEntity?> GetByIdAsync(Guid id)
        {
            return await getUserByIdService.Run(id);
        }

        public async Task<UserEntity> CreateAsync(UserEntity user)
        {
            return await createUserService.Run(user);
        }

        public async Task<UserEntity?> DeleteByIdAsync(Guid id)
        {
            return await deleteUserByIdService.Run(id);
        }

        public async Task<UpdateUserResultDto> UpdateByIdAsync(Guid id, UpdateUserDto user)
        {
            return await updateUserByIdService.Run(id, user);
        }

        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            return await getUserByEmailService.Run(email);
        }
        public async Task DeleteUsers(List<string> userEmails) { 
            await deleteUsers.Run(userEmails);
        }

        public List<SecurityQuestionDto> GetSecurityQuestions()
        {
            List<SecurityQuestionDto> securityQuestions = [];
            foreach (SecurityQuestion item in Enum.GetValues(typeof(SecurityQuestion)))
            {
                securityQuestions.Add(
                    new SecurityQuestionDto 
                    {
                        Id = (int)item,
                        Question = item.GetSecurityCuestionString(),
                    }
                );
            };
            return securityQuestions;
        }

    }
}