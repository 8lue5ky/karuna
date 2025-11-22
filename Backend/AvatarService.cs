using Backend.Controller.Users;

namespace Backend
{
    public class AvatarService : IAvatarService
    {
        private readonly AvatarGenerator _avatarGenerator = new AvatarGenerator();

        public async Task CreateAvatar(string username, string userId)
        {
            byte[] avatar = _avatarGenerator.GenerateAvatar(username);

            await SaveUserImageAsync(userId, avatar);
        }

        public async Task SaveUserImageAsync(string userId, byte[] imageData, string fileName = "profile.png")
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "users", userId);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            await File.WriteAllBytesAsync(filePath, imageData);
        }

        public async Task SaveAvatar(IFormFile formFile, string userId)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "users", userId);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, "profile.png");

            using var stream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(stream);
        }
    }
}
