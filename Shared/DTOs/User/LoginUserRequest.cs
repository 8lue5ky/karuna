using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.User
{
    public class LoginUserRequest
    {
        public required string EmailOrUsername { get; set; }
        public required string Password { get; set; }
    }
}
