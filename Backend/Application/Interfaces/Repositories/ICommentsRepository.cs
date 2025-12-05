using Backend.Domain.Models.Posts;
using Shared.DTOs.Comments;

namespace Backend.Application.Interfaces.Repositories;

public interface ICommentsRepository
{
    Task CreateCommentAsync(Comment comment);
    Task<IReadOnlyList<CommentDto>> GetCommentsAsync(Guid postId);
    Task DeleteAsync(Guid id);
}