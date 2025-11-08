using Backend.Models.User;

namespace Backend.Persistence.User;

internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetUserProfileAsync(Guid id)
    {
        return await _context.UserProfiles.FindAsync(id);
    }
}