using Microsoft.AspNetCore.Identity;

namespace Backend.Domain.Models.User;

public class AppUser : IdentityUser
{
    public UserProfile? Profile { get; set; }
}
