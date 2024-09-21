using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Session.domain.entity;
using backend.Data;

namespace Api.src.Session.application.service
{
    public class CreateSession
    {
        private readonly ApplicationDBContext _context;

        public CreateSession(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<SessionEntity> Run(SessionEntity session)
        {
            await _context.Session.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }
    }
}