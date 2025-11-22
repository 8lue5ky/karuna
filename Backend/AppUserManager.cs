using Backend.Models.User;
using Backend.Persistence.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Backend
{
    internal class AppUserManager : UserManager<AppUser>
    {
        private readonly AppDbContext _dbContext;
        private readonly AvatarGenerator _avatarGenerator = new AvatarGenerator();

        public AppUserManager(IUserStore<AppUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<AppUser> passwordHasher, 
            IEnumerable<IUserValidator<AppUser>> userValidators, 
            IEnumerable<IPasswordValidator<AppUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<AppUser>> logger,
            AppDbContext dbContext) : 
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _dbContext = dbContext;
        }

        // TODO: Move to controller?
        public override async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            var result = await base.CreateAsync(user, password);
            if (result.Succeeded)
            {
                byte[] avatar = _avatarGenerator.GenerateAvatar(user.UserName);

                await SaveUserImageAsync(user.Id, avatar);

                UserProfile profile = new UserProfile()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id
                };

                await _dbContext.UserProfiles.AddAsync(profile);
                await _dbContext.SaveChangesAsync();
            }
            return result;
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
    }
}
