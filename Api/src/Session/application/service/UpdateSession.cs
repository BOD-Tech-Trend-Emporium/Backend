using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Common.exceptions;
using Api.src.Session.domain.entity;
using backend.Data;

namespace Api.src.Session.application.service
{
    public class UpdateSession
    {
        private readonly ApplicationDBContext _context;

        public UpdateSession(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<SessionEntity?> Run(Guid id, bool sessionValue, DateTime currentTime)
        {
            var sessionModel = await _context.Session.FindAsync(id);

            if(sessionModel == null)
            {
                throw new NotFoundException($"User with id {id} not found");
            }

            sessionModel.IsActive = sessionValue;
            sessionModel.CreatedAt = currentTime;

            await _context.SaveChangesAsync();
            return sessionModel;
        }
    }
}