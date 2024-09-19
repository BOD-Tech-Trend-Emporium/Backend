using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.Auth.domain.dto
{
    public class LoggedUserDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}