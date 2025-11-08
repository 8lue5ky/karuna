using Backend.Models.User;

namespace Backend.Persistence.User
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetUserProfileAsync(string userId);
    }
}