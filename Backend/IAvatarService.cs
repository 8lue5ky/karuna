namespace Backend;

public interface IAvatarService
{
    Task CreateAvatar(string username, string userId);
    Task SaveAvatar(IFormFile formFile, string userId);
}