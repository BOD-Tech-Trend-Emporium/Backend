using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.Auth.domain.dto
{
    public class TokenDto
    {
        public string UserId {get; set;}
        public string Email {get; set;}
        public string Role {get; set;}

    }
}