using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.User
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
    }
}
