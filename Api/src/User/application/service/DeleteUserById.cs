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

        public DeleteUserById(ApplicationDBContext context)
        {
            _context = context;
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

            return user;
        }
    }
}