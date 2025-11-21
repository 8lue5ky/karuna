using Backend.Models.Posts;
using Backend.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace Backend;

public class SeedData
{
    private static readonly AvatarGenerator AvatarGenerator = new AvatarGenerator();

    private static readonly IEnumerable<SeedUser> seedUsers =
    [
        new SeedUser()
        {
            Email = "leela@contoso.com", 
            NormalizedEmail = "LEELA@CONTOSO.COM", 
            NormalizedUserName = "LEELA@CONTOSO.COM", 
            RoleList = [ "Administrator", "Manager" ], 
            UserName = "leela@contoso.com",
            DisplayName = "Leela"
        },
        new SeedUser()
        {
            Email = "harry@contoso.com",
            NormalizedEmail = "HARRY@CONTOSO.COM",
            NormalizedUserName = "HARRY@CONTOSO.COM",
            RoleList = [ "User" ],
            UserName = "harry@contoso.com",
            DisplayName = "Harry"
        },
    ];

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

        if (context.Users.Any())
        {
            return;
        }

        var userStore = new UserStore<AppUser>(context);
        var password = new PasswordHasher<AppUser>();

        using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = [ "Administrator", "Manager", "User" ];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        foreach (var user in seedUsers)
        {
            var hashed = password.HashPassword(user, "Passw0rd!");
            user.PasswordHash = hashed;
            await userStore.CreateAsync(user);

            if (user.Email is not null)
            {
                var appUser = await userManager.FindByEmailAsync(user.Email);

                if (appUser is not null && user.RoleList is not null)
                {
                    await userManager.AddToRolesAsync(appUser, user.RoleList);

                    UserProfile userProfile = new UserProfile()
                    {
                        Bio = "sdgsdg",
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        Location = "Tenesee",
                        ProfileImageThumbnail = AvatarGenerator.GenerateAvatarAsync(appUser.UserName)
                    };

                    context.UserProfiles.Add(userProfile);

                    Post[] posts =
                    {
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Lorem ipsum",
                            UserId = appUser.Id,
                            Description =
                                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et e"
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Lorem ipsum",
                            UserId = appUser.Id,
                            Description =
                                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et e"
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Lorem ipsum",
                            UserId = appUser.Id,
                            Description =
                                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et e"
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Lorem ipsum",
                            UserId = appUser.Id,
                            Description =
                                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et e"
                        },
                    };

                    context.Posts.AddRange(posts);
                }
            }



        }

        await context.SaveChangesAsync();
    }

    private class SeedUser : AppUser
    {
        public string[]? RoleList { get; set; }
    }

    public static async Task<byte[]> GenerateAvatarAsync(string initials, int size = 128)
    {
        using var bitmap = new SKBitmap(size, size);
        using var canvas = new SKCanvas(bitmap);

        // Hintergrundfarbe
        var paint = new SKPaint
        {
            Color = SKColor.Parse("#3F51B5"),
            IsAntialias = true
        };
        canvas.DrawCircle(size / 2, size / 2, size / 2, paint);

        // Text
        var textPaint = new SKPaint
        {
            Color = SKColors.White,
            TextAlign = SKTextAlign.Center,
            TextSize = size * 0.5f,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Arial")
        };

        canvas.DrawText(initials, size / 2, (size / 2) + (textPaint.TextSize / 3), textPaint);

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        return data.ToArray();
    }
}
