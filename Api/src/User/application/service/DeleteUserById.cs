using Api.src.Auth.application.service;
using Api.src.Common.exceptions;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using Microsoft.EntityFrameworkCore;

namespace backend.src.User.application.service
{
    public class DeleteUserById
    {
        private readonly ApplicationDBContext _context;
        private readonly LogoutUser _logoutUser;

        public DeleteUserById(ApplicationDBContext context)
        {
            _context = context;
            _logoutUser = new LogoutUser(context);
        }
        public async Task<UserEntity?> Run(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            Console.WriteLine(user);
            if(user == null || user.Status == UserStatus.Removed)
            {
                throw new NotFoundException($"User with id {id} not found");
            }

            user.Status = UserStatus.Removed;
            await _context.SaveChangesAsync();
            await _logoutUser.Run(id.ToString());
            return user;
        }
    }
}