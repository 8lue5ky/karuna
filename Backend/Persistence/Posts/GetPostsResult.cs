using Backend.Models;
using Shared.DTOs.Posts;

namespace Backend.Persistence.Posts
{
    public class GetPostsResult
    {
        public required IReadOnlyList<PostDto> Posts { get; set; }

        public bool HasMore { get; set; }
    }
}
