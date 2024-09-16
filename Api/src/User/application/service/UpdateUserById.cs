using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.src.User.application.mappers;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;

namespace backend.src.User.application.service
{
    public class UpdateUserById
    {
        private readonly ApplicationDBContext _context;
        private GetUserById getUserByIdService;


        public UpdateUserById(ApplicationDBContext context)
        {
            _context = context;
            getUserByIdService = new GetUserById(context);

        }
        public async Task<UserEntity?> Run(int id, UpdateUserDto user)
        {
            var userModel = await getUserByIdService.Run(id);
            
            if(userModel == null)
            {
                return null;
            }

            userModel.Name = user.Name;
            userModel.Age = user.Age;

            await _context.SaveChangesAsync();

            return userModel;
        }
    }
}