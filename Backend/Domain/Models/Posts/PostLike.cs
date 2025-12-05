using Backend.Domain.Models.User;

namespace Backend.Domain.Models.Posts
{
    public class PostLike
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}
