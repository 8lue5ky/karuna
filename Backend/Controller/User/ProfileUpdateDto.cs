using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller.User;

public class ProfileUpdateDto
{
    [FromForm(Name = "Username")]
    public string? Username { get; set; }

    [FromForm(Name = "Email")]
    public string? Email { get; set; }

    [FromForm(Name = "Bio")]
    public string? Bio { get; set; }

    [FromForm(Name = "Location")]
    public string? Location { get; set; }

    [FromForm(Name = "ProfileImage")]
    public IFormFile? ProfileImage { get; set; }
}