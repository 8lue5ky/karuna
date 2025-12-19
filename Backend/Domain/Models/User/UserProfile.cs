using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Models.User
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        [MaxLength(3000)]
        public string? Bio { get; set; }

        public string UserId { get; set; } = string.Empty;

        public AppUser User { get; set; } = default!;

        [MaxLength(250)]
        public string? Location { get; set; }
    }
}
