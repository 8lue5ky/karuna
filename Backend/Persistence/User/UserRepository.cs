using Backend.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence.User;

internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<UserProfile?> GetUserProfileAsync(string userId)
    {
        return await _context.UserProfiles
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<byte[]?> GetProfileImageThumbnailAsync(string userId)
    {
        return await _context.UserProfiles
            .Where(x => x.UserId == userId)
            .Select(x => x.ProfileImageThumbnail)
            .FirstOrDefaultAsync();
    }

    public async Task<IdentityResult> UpdateProfileAsync(string userId, UpdateProfileAction action)
    {
        UserProfile? profile = await _context.UserProfiles
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (profile == null)
        {
            // TODO: Create new profile
            return IdentityResult.Failed();
        }

        profile.Bio = action.Bio;
        profile.Location = action.Location;
        profile.User.Email = action.Email;
        profile.User.UserName = action.DisplayName;

        //profile.User.NormalizedEmail = _userManager.NormalizeEmail(action.Email);
        //profile.User.NormalizedUserName = _userManager.NormalizeName(action.DisplayName);

        if (action.ProfilePicture != null)
        {
            profile.ProfileImageThumbnail = action.ProfilePicture;
        }

        var identityResult = await _userManager.UpdateAsync(profile.User);

        if (!identityResult.Succeeded)
        {
            return identityResult;
        }

        await _context.SaveChangesAsync();

        return IdentityResult.Success;
    }

    public async Task CreateUserProfileAsync(UserProfile userProfile)
    {
        await _context.UserProfiles.AddAsync(userProfile);
        await _context.SaveChangesAsync();
    }
}