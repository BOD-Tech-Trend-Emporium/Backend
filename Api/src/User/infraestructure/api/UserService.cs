using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.src.User.application.service;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;
using backend.src.User.domain.repository;
using Microsoft.EntityFrameworkCore;

namespace backend.src.User.infraestructure.api
{
    public class UserService: UserRepository
    {
        private GetAllUsers getAllUsersService;
        private CreateUser createUserService;
        private DeleteUserById deleteUserByIdService;
        private GetUserById getUserByIdService;
        private UpdateUserById updateUserByIdService;

        public UserService(ApplicationDBContext context)
        {
            getAllUsersService = new GetAllUsers(context);
            createUserService = new CreateUser(context);
            deleteUserByIdService = new DeleteUserById(context);
            getUserByIdService = new GetUserById(context);
            updateUserByIdService = new UpdateUserById(context);
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

        public async Task<UserEntity?> UpdateByIdAsync(Guid id, UpdateUserDto user)
        {
            return await updateUserByIdService.Run(id, user);
        }
    }
}