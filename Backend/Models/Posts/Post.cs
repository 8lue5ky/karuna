using Backend.Models.User;

namespace Backend.Models.Posts
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; }

        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    }
}
