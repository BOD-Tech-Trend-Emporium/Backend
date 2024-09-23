using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Session.application.validations
{
    public class SessionValidations
    {
        private readonly ApplicationDBContext _context;
        public SessionValidations(ApplicationDBContext context){
            _context = context;
        }

        public async Task<bool> IsUserLoggedIn(Guid id)
        {
            return await _context.Session.AnyAsync(x => x.UserId == id && x.IsActive == true);
        }
    }
}