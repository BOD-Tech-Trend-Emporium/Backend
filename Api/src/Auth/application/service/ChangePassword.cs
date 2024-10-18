using Api.src.Auth.domain.dto;
using Api.src.Common.exceptions;
using Api.src.User.application.service;
using Api.src.User.domain.enums;
using backend.Data;
using backend.src.User.domain.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.src.Auth.application.service
{
    public class ChangePassword
    {
        private readonly ApplicationDBContext _context;
        private IConfiguration _configuration;
        private GetUserByEmail getUserByEmailService;
        public ChangePassword(ApplicationDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            getUserByEmailService = new GetUserByEmail(context);

        }
        public async Task<UserEntity>Run(ChangePasswordDto toUpdate)
        {
            UserEntity user = await getUserByEmailService.Run(toUpdate.email);
            if (user == null)
            {
                throw new NotFoundException($"User with email {toUpdate.email} not found");
            }
            if ((SecurityQuestion)toUpdate.SecurityQuestion != user.SecurityQuestion ||
                toUpdate.SecurityQuestionAnswer != user.SecurityQuestionAnswer) 
            {
                throw new BadRequestException("invalid question or answer");
            }
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(toUpdate.password);
            user.Password = passwordHash;

            await _context.SaveChangesAsync();
            return user;
        }
    }
}
