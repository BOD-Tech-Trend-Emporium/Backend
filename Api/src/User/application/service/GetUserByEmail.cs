using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using Microsoft.EntityFrameworkCore;

namespace Api.src.User.application.service
{
    public class GetUserByEmail
    {
        private readonly ApplicationDBContext _context;

        public GetUserByEmail(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> Run(string email)
        {
            return await _context.User.FirstOrDefaultAsync(x => x.Email == email && x.Status == UserStatus.Created);
        }
    }
}