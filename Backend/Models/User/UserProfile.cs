namespace Backend.Models.User
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        public byte[]? ProfileImageThumbnail { get; set; }

        public string? Bio { get; set; }

        public string UserId { get; set; } = string.Empty;

        public AppUser User { get; set; } = default!;

        public string? Location { get; set; }
    }
}
