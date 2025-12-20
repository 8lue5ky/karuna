using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Models.Posts;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Posts;

namespace Backend.Infrastructure.Persistence
{
    internal class PostsRepository : IPostsRepository
    {
        private readonly AppDbContext _context;

        public PostsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreatePostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task<GetPostsResult> GetPostsAsyncOrderedByCreated(int pageSize, int skip, PostType type, string language, string? userId)
        {
            var items = await _context.Posts
                .Include(x => x.User)
                .Where(x => x.Type == type && x.Language == language)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new PostDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    Username = x.User != null ? x.User.UserName : null,
                    UserId = x.UserId,
                    LikeCount = _context.PostLikes.Count(l => l.PostId == x.Id),
                    CommentCount = _context.Comments.Count(c => c.PostId == x.Id),
                    HasLiked = userId == null
                        ? null
                        : _context.PostLikes.Any(l => l.PostId == x.Id && l.UserId == userId)
                })
                .ToListAsync();

            bool hasMore = skip + pageSize < _context.Posts.Count(x => x.Type == type && x.Language == language);

            return new GetPostsResult()
            {
                Posts = items,
                HasMore = hasMore
            };
        }

        public async Task LikePostAsync(Guid postId, string userId)
        {
            _context.PostLikes.Add(new PostLike
            {
                PostId = postId,
                UserId = userId
            });

            await _context.SaveChangesAsync();
        }

        public async Task UnlikePostAsync(Guid postId, string userId)
        {
            PostLike? like = await _context.PostLikes.FindAsync(postId, userId);

            if (like == null)
            {
                return;
            }

            _context.PostLikes.Remove(like);
            await _context.SaveChangesAsync();
        }

        public async Task<PostDto?> GetPostAsync(Guid id)
        {
            return await _context.Posts
                .Where(p => p.Id == id)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    Username = p.User != null ? p.User.UserName : null,
                    UserId = p.UserId,
                    LikeCount = _context.PostLikes.Count(l => l.PostId == p.Id),
                    CommentCount = _context.Comments.Count(c => c.PostId == p.Id)
                })
                .FirstOrDefaultAsync();
        }

        public async Task DeletePost(Guid id)
        {
            Post? post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
