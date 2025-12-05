using Backend.Domain.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetUserProfileAsync(string userId);
        Task<IdentityResult> UpdateProfileAsync(string userId, UpdateProfileAction action);
        Task CreateUserProfileAsync(UserProfile userProfile);
    }
}