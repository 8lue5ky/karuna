using Backend.Models.User;

namespace Backend.Persistence.User
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetUserProfileAsync(Guid id);
    }
}