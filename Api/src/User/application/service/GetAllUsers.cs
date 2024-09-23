using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using Microsoft.EntityFrameworkCore;

namespace backend.src.User.application.service
{
    public class GetAllUsers
    {
        private readonly ApplicationDBContext _context;

        public GetAllUsers(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<UserEntity>> Run()
        {
            return await _context.User.Where(user => user.Role != UserRole.Admin && user.Status != UserStatus.Removed).ToListAsync();
        }
    }
}