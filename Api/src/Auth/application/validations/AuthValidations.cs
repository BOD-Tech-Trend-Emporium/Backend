using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Auth.application.validations
{
    public class AuthValidations
    {
        private readonly ApplicationDBContext _context;
        public AuthValidations(ApplicationDBContext context){
            _context = context;
        }
        public async Task<bool> EmailExists(string email)
        {
            return await _context.User.AnyAsync(x => x.Email == email && x.Status == UserStatus.Created);
        }

        public async Task<bool> UserNameExists(string userName)
        {
            return await _context.User.AnyAsync(x => x.UserName == userName && x.Status == UserStatus.Created);
        }

        public bool IsEmailValid(string email)
        {
            string EmailRegex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            return Regex.IsMatch(email, EmailRegex, RegexOptions.IgnoreCase);
        }

        public bool IsRoleValid(UserRole role)
        {
            return Enum.IsDefined(typeof(UserRole), role);
        }
    }
}