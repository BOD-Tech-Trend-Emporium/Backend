using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.User.domain.dto
{
    public class UpdateUserResultDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
    }
}