using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.src.User.domain.entity;

namespace backend.src.User.application.service
{
    public class CreateUser
    {
        private readonly ApplicationDBContext _context;

        public CreateUser(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<UserEntity> Run(UserEntity user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}