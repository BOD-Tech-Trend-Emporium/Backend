using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.Session.domain.dto
{
    public class CreateSessionDto
    {
        public Guid UserId {get; set;}
        public bool IsActive { get; set; }
    }
}