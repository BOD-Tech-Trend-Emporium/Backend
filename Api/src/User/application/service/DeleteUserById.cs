using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.src.User.domain.entity;
using Microsoft.EntityFrameworkCore;

namespace backend.src.User.application.service
{
    public class DeleteUserById
    {
        private readonly ApplicationDBContext _context;
        private GetUserById getUserByIdService;

        public DeleteUserById(ApplicationDBContext context)
        {
            _context = context;
            getUserByIdService = new GetUserById(context);
        }
        public async Task<UserEntity?> Run(int id)
        {
            var user = await getUserByIdService.Run(id);
            
            if(user == null)
            {
                return null;
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}