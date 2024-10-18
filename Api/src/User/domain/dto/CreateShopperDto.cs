using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.src.User.domain.enums;

namespace Api.src.User.domain.dto
{
    public class CreateShopperDto
    {
        public string Email { get; set; }
        public string Name {get; set;}
        public string UserName { get; set; }
        public string Password { get; set; }
        public int SecurityQuestion { get; set; }
        public string SecurityQuestionAnswer { get; set; }
    }
}