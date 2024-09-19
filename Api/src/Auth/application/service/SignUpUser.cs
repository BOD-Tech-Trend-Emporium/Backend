using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using backend.Data;
using backend.src.User.domain.entity;
using BCrypt.Net;

namespace Api.src.Auth.application.service
{
    public class SignUpUser
    {
        private readonly ApplicationDBContext _context;

        public SignUpUser(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> Run(UserEntity user)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passwordHash;
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}