using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.src.User.domain.dto;
using backend.src.User.domain.entity;

namespace backend.src.User.domain.repository
{
    public interface UserRepository
    {
        Task<List<UserEntity>> GetAllAsync();
        Task<UserEntity?> GetByIdAsync(Guid id);
        Task<UserEntity?> GetByEmailAsync(string email);
        Task<UserEntity> CreateAsync(UserEntity user);
        Task<UserEntity?> DeleteByIdAsync(Guid id);
        Task<UserEntity?> UpdateByIdAsync(Guid id, UpdateUserDto user);
        Task<List<UserEntity>> DeleteUsers(List<string> userEmails);
    }
}