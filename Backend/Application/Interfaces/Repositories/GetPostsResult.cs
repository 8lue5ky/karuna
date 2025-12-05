using Shared.DTOs.Posts;

namespace Backend.Application.Interfaces.Repositories
{
    public class GetPostsResult
    {
        public required IReadOnlyList<PostDto> Posts { get; set; }

        public bool HasMore { get; set; }
    }
}
