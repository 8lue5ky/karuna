namespace Backend.Application.Interfaces.Repositories;

public class UpdateProfileAction
{
    public byte[]? ProfilePicture { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? Bio { get; set; }
    public string? Location { get; set; }
}