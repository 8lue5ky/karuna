using Backend.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Backend.Persistence.User
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetUserProfileAsync(string userId);
        Task<byte[]?> GetProfileImageThumbnailAsync(string userId);
        Task<IdentityResult> UpdateProfileAsync(string userId, UpdateProfileAction action);
        Task CreateUserProfileAsync(UserProfile userProfile);
    }
}