using Backend.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<GetPostsResult> GetPostsAsyncOrderedByCreated(int pageSize, int skip)
        {
            var items = await _context.Posts
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            bool hasMore = skip + pageSize < _context.Posts.Count();

            return new GetPostsResult()
            {
                Posts = items,
                HasMore = hasMore
            };
        }
    }
}
