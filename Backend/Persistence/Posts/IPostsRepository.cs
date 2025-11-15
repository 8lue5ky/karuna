using Backend.Models.Posts;

namespace Backend.Persistence.Posts;

public interface IPostsRepository
{
    Task CreatePostAsync(Post post);
    Task<GetPostsResult> GetPostsAsyncOrderedByCreated(int pageSize, int skip, string? userId = null);
    Task LikePostAsync(Guid postId, string userId);
    Task UnlikePostAsync(Guid postId, string userId);
}