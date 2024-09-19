using Api.src.Common.exceptions;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
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

        public async Task Run(List<string> userEmails)
        {
            var usersToDelete = await _context.User.Where(user => userEmails.Contains(user.Email) && user.Status != UserStatus.Removed).ToListAsync();

            if (userEmails.IsNullOrEmpty() || usersToDelete.IsNullOrEmpty() || usersToDelete.Count() != userEmails.Count())
            {
                var notFoundUsers = userEmails.Where(userEmails => !usersToDelete.Any(userToDelete => userEmails == userToDelete.Email));

                throw new NotFoundException("Users/emails do not exist: {0}", String.Join(" ", notFoundUsers));
            }

            var result=usersToDelete.Select(userToDelete => {
                userToDelete.Status = UserStatus.Removed;
                return userToDelete;
            }).ToList();

            await _context.SaveChangesAsync();

        }
    }
}
