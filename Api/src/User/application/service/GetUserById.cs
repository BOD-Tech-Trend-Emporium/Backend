using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.src.User.domain.entity;
using Microsoft.EntityFrameworkCore;

namespace backend.src.User.application.service
{
    public class GetUserById
    {
        private readonly ApplicationDBContext _context;

        public GetUserById(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<UserEntity?> Run(int id)
        {
            return await _context.User.FindAsync(id);
        }
    }
}