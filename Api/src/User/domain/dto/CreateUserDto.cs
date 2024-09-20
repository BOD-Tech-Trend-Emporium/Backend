using Api.src.User.domain.enums;
using backend.src.User.domain.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.src.User.domain.dto
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Name {get; set;}
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}