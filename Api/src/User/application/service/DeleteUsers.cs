using Api.src.Common.exceptions;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace Api.src.User.application.service
{
    public class DeleteUsers
    {
        private readonly ApplicationDBContext _context;

        public DeleteUsers(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<UserEntity>> Run(List<string> userEmails)
        {
            var usersToDelete = await _context.User.Where(user => userEmails.Contains(user.Email) && user.Status != UserStatus.Removed).ToListAsync();
            
            if (userEmails.IsNullOrEmpty() || usersToDelete.IsNullOrEmpty())
            {
                throw new NotFoundException("Users/emails do not exist: {0}", String.Join(" ", userEmails));
            }
            if (usersToDelete.Count() != userEmails.Count())
            {
                return null;
            }


            var result=usersToDelete.Select(userToDelete => {
                userToDelete.Status = UserStatus.Removed;
                return userToDelete;
            }).ToList();

            await _context.SaveChangesAsync();

            return result;
        }
    }
}
