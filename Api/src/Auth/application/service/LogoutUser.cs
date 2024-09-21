using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.validations;
using Api.src.Common.exceptions;
using Api.src.Session.application.service;
using Api.src.Session.application.validations;
using Api.src.Session.domain.entity;
using backend.Data;
using backend.src.User.application.service;
using backend.src.User.domain.entity;

namespace Api.src.Auth.application.service
{
    public class LogoutUser
    {
        private readonly ApplicationDBContext _context;
        private UpdateSession updateSessionService;
        private SessionValidations sessionValidations;


        public LogoutUser(ApplicationDBContext context)
        {
            _context = context;
            updateSessionService = new UpdateSession(context);
            sessionValidations = new SessionValidations(context);
        }

        public async Task<SessionEntity?> Run(string id)
        {
            Guid userId = Guid.Parse(id);

            var isLoggedIn = await sessionValidations.IsUserLoggedIn(userId);

            if(!isLoggedIn)
            {
                throw new ConflictException("User doesn't have an active session");
            }

            var sessionModel = await updateSessionService.Run(userId, false, DateTime.Now);
            return sessionModel;
        }
    }
}