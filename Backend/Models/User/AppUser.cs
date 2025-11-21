using Microsoft.AspNetCore.Identity;

namespace Backend.Models.User;

public class AppUser : IdentityUser
{
    public UserProfile? Profile { get; set; }
}
