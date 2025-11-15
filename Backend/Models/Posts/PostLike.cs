using Backend.Models.User;

namespace Backend.Models.Posts
{
    public class PostLike
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}
