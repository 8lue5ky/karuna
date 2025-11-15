using Backend.Models.User;

namespace Backend.Models.Posts
{
    public class Post
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    }
}
