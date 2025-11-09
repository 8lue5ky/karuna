using Backend.Models.User;

namespace Backend.Persistence.User
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetUserProfileAsync(string userId);
        Task<byte[]?> GetProfileImageThumbnailAsync(string userId);
        Task UpdateProfileAsync(string userId, UpdateProfileAction action);
    }
}