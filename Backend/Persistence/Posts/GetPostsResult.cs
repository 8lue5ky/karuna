using Backend.Models;

namespace Backend.Persistence.Posts
{
    public class GetPostsResult
    {
        public required IReadOnlyList<Post> Posts { get; set; }

        public bool HasMore { get; set; }
    }
}
