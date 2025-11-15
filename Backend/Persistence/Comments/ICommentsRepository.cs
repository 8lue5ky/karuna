using Backend.Models.Posts;
using Shared.DTOs.Comments;

namespace Backend.Persistence.Comments;

public interface ICommentsRepository
{
    Task CreateCommentAsync(Comment comment);
    Task<IReadOnlyList<CommentDto>> GetCommentsAsync(Guid postId);
    Task DeleteAsync(Guid id);
}