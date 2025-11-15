using Backend.Models.Posts;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Posts;

namespace Backend.Persistence.Posts
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

        public async Task<GetPostsResult> GetPostsAsyncOrderedByCreated(int pageSize, int skip, string? userId)
        {
            var items = await _context.Posts
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new PostDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    Username = x.User.UserName,

                    LikeCount = _context.PostLikes.Count(l => l.PostId == x.Id),
                    HasLiked = userId == null
                        ? null
                        : _context.PostLikes.Any(l => l.PostId == x.Id && l.UserId == userId)
                })
                .ToListAsync();

            bool hasMore = skip + pageSize < _context.Posts.Count();

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
    }
}
