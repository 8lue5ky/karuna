using Backend.Domain.Models.User;
using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Models.Posts
{
    public class Comment
    {
        public Guid Id { get; set; }

        [MaxLength(3000)]
        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; } = null!;

        public AppUser User { get; set; } = null!;

        public Guid PostId { get; set; }

        public Post Post { get; set; } = null!;
    }
}
