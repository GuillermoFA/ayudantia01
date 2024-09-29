using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos.Auth
{
    public class LoginDto
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}