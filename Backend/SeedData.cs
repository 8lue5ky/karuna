using Backend.Models.Posts;
using Backend.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace Backend;

public class SeedData
{
    private static readonly AvatarService AvatarGenerator = new AvatarService();

    private static readonly IEnumerable<SeedUser> seedUsers =
    [
        new SeedUser()
        {
            Email = "leela@contoso.com", 
            NormalizedEmail = "LEELA@CONTOSO.COM", 
            NormalizedUserName = "LEELA", 
            RoleList = [ "Administrator", "Manager" ], 
            UserName = "Leela",
        },
        new SeedUser()
        {
            Email = "harry@contoso.com",
            NormalizedEmail = "HARRY@CONTOSO.COM",
            NormalizedUserName = "HARRY",
            RoleList = [ "User" ],
            UserName = "Harry"
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

                    await AvatarGenerator.CreateAvatar(appUser.UserName, appUser.Id);

                    UserProfile userProfile = new UserProfile()
                    {
                        Bio = "sdgsdg",
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        Location = "Tenesee"
                    };

                    context.UserProfiles.Add(userProfile);

                    Post[] posts =
                    {
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Einkauf für meine ältere Nachbarin erledigt",
                            UserId = appUser.Id,
                            Description =
                                "Meine ältere Nachbarin hat seit ein paar Tagen eine starke Erkältung und wollte eigentlich jemanden bitten, ihr Medikamente zu holen. Als ich sie zufällig im Treppenhaus traf, sah sie wirklich erschöpft aus. Ich habe ihr angeboten, nicht nur Medikamente, sondern gleich den kompletten Wocheneinkauf zu erledigen. Sie gab mir eine kleine Liste, und als ich zurückkam, konnte man ihr richtig ansehen, wie erleichtert sie war. Sie meinte, solche Gesten geben ihr das Gefühl, dass sie noch Teil einer Gemeinschaft ist. Ein schöner Moment."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Nachhilfe für einen überforderten Schüler",
                            UserId = appUser.Id,
                            Description = "Ein Nachbarsjunge hatte große Mühe in Mathe und war völlig frustriert. Ich habe mich eine Stunde hingesetzt und mit ihm die Grundlagen wiederholt. Am Ende hat er wirklich verstanden, wie die Aufgabe funktioniert. Seine Augen haben geleuchtet – und er sagte, das sei das erste Mal, dass Mathe ihm nicht Angst macht. Das war ein schönes Gefühl."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Kleines Café-Experiment: Kaffee für die Person hinter mir",
                            UserId = appUser.Id,
                            Description =
                                "Ich habe spontan beschlossen, im Café den Kaffee für die Person hinter mir mitzubezahlen. Ich sagte dem Barista, er solle einfach sagen, „heute ist jemand nett gewesen“. Als ich später aus dem Fenster sah, konnte ich beobachten, wie sich die Frau riesig gefreut hat. Einfach schön zu sehen, wie kleine Gesten wirken."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Im Park Müll gesammelt",
                            UserId = appUser.Id,
                            Description =
                                "Ich war auf einem kurzen Spaziergang und habe gemerkt, wie viel Müll auf den Wiesen liegt. Ich nahm mir zehn Minuten Zeit, sammelte ein paar Flaschen und Verpackungen auf und war überrascht, wie groß der Unterschied danach schon war. Ein Paar auf einer Bank hat mir sogar zugelächelt und „Danke“ gesagt. Hat sich gut angefühlt."
                        },
                    };

                    context.Posts.AddRange(posts);

                    foreach (int i in Enumerable.Range(0, 100))
                    {
                        var post = new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Im Park Müll gesammelt",
                            UserId = appUser.Id,
                            Description =
                                "Ich war auf einem kurzen Spaziergang und habe gemerkt, wie viel Müll auf den Wiesen liegt. Ich nahm mir zehn Minuten Zeit, sammelte ein paar Flaschen und Verpackungen auf und war überrascht, wie groß der Unterschied danach schon war. Ein Paar auf einer Bank hat mir sogar zugelächelt und „Danke“ gesagt. Hat sich gut angefühlt."
                        };

                        context.Posts.Add(post);
                    }
                }
            }



        }

        await context.SaveChangesAsync();
    }

    private class SeedUser : AppUser
    {
        public string[]? RoleList { get; set; }
    }
}
