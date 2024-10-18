using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.User.domain.dto;
using Api.src.User.domain.enums;
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
        Task<UpdateUserResultDto> UpdateByIdAsync(Guid id, UpdateUserDto user);
        Task DeleteUsers(List<string> userEmails);
        List<SecurityQuestionDto> GetSecurityQuestions();
    }
}