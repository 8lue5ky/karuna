using Backend.Models.Posts;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Comments;

namespace Backend.Persistence.Comments
{
    internal class CommentsRepository : ICommentsRepository
    {
        private readonly AppDbContext _context;

        public CommentsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<CommentDto>> GetCommentsAsync(Guid postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    PostId = c.PostId,
                    UserName = c.User.DisplayName
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
