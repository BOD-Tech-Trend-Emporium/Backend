using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.Auth.domain.dto
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password {get; set;}
    }
}