using Backend.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence.User;

internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetUserProfileAsync(string userId)
    {
        return await _context.UserProfiles
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}