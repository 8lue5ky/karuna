using Backend.Application.Avatars;
using Backend.Domain.Models.User;
using Backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Backend.Application
{
    internal class AppUserManager : UserManager<AppUser>
    {
        private readonly AppDbContext _dbContext;
        private readonly IAvatarService _avatarService;
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
            AppDbContext dbContext,
            IAvatarService avatarService) : 
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _dbContext = dbContext;
            _avatarService = avatarService;
        }

        // TODO: Move to controller?
        public override async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            var result = await base.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _avatarService.CreateAvatar(user.UserName, user.Id);

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

    }
}
