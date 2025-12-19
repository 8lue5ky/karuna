using Backend.Domain.Models.User;
using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Models.Posts
{
    public class Post
    {
        public Guid Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; } = null!;

        [MaxLength(3000)]
        public string Description { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string? UserId { get; set; }

        public AppUser? User { get; set; }

        public PostType Type { get; set; }

        [MaxLength(20)]
        public string Language { get; set; }

        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    }
}
