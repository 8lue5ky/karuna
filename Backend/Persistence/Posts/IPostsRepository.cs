using Backend.Models;

namespace Backend.Persistence.Posts;

public interface IPostsRepository
{
    Task CreatePostAsync(Post post);
    Task<GetPostsResult> GetPostsAsyncOrderedByCreated(int pageSize, int skip);
}