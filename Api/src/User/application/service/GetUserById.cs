using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Api.src.Common.exceptions;
using backend.Data;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
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
        public async Task<UserEntity?> Run(Guid id)
        {
            var user =  await _context.User.FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with id {id} not found");
            }
            return user;
        }
    }
}