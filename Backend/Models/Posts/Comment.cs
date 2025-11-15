using Backend.Models.User;

namespace Backend.Models.Posts
{
    public class Comment
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;

        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;
    }
}
