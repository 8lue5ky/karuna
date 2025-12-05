using Backend.Domain.Models.Posts;
using Shared.DTOs.Posts;

namespace Backend.Application.Interfaces.Repositories;

public interface IPostsRepository
{
    Task CreatePostAsync(Post post);
    Task<GetPostsResult> GetPostsAsyncOrderedByCreated(int pageSize, int skip, PostType type, string? userId = null);
    Task LikePostAsync(Guid postId, string userId);
    Task UnlikePostAsync(Guid postId, string userId);
    Task<PostDto?> GetPostAsync(Guid id);
}