using Microsoft.AspNetCore.Identity;

namespace Backend.Models.User;

public class AppUser : IdentityUser
{
    public string? DisplayName { get; set; }

    public UserProfile? Profile { get; set; }
}
