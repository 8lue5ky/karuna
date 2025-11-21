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
                byte[] avatar = _avatarGenerator.GenerateAvatarAsync(user.UserName);

                UserProfile profile = new UserProfile()
                {
                    Id = Guid.NewGuid(),
                    ProfileImageThumbnail = avatar,
                    UserId = user.Id
                };

                await _dbContext.UserProfiles.AddAsync(profile);
                await _dbContext.SaveChangesAsync();
            }
            return result;
        }
    }
}
