namespace Backend.Models.User
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        public byte[]? ProfileImageThumbnail { get; set; }

        public string? Bio { get; set; }
    }
}
