using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.mappers;
using Api.src.Auth.application.Utils;
using Api.src.Auth.domain.dto;
using Api.src.Common.exceptions;
using Api.src.Session.application.service;
using Api.src.Session.application.validations;
using Api.src.User.application.service;
using backend.Data;

namespace Api.src.Auth.application.service
{
    public class LoginUser
    {
        private readonly ApplicationDBContext _context;
        private IConfiguration _configuration;
        private GetUserByEmail getUserByEmailService;
        private UpdateSession updateSessionService;
        private SessionValidations sessionValidations;


        public LoginUser(ApplicationDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            getUserByEmailService = new GetUserByEmail(context);
            updateSessionService = new UpdateSession(context);
            sessionValidations = new SessionValidations(context);
        }

        public async Task<LoggedUserDto?> Run(UserLoginDto user)
        {
            var userModel = await getUserByEmailService.Run(user.Email);
            var passwordsMatch = false;

            if(userModel != null)
            {
                passwordsMatch = BCrypt.Net.BCrypt.Verify(user.Password, userModel.Password);
            }

            if(userModel == null || !passwordsMatch)
            {
                throw new BadRequestException("Wrong email or password");
            }

            var isLoggedIn = await sessionValidations.IsUserLoggedIn(userModel.Id);

            if(isLoggedIn)
            {
                throw new ConflictException("User already logged in");
            }

            string tokenKey = _configuration.GetSection("AppSettings:TokenKey").Value!;
            string token = Token.CreateToken(userModel, tokenKey);

            var loggedUser = AuthMappers.ToLoggedUser(userModel, token);
            await updateSessionService.Run(userModel.Id, true, DateTime.Now);

            return loggedUser;
        }
    }
}