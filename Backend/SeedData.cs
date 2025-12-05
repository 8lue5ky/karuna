using Backend.Application.Avatars;
using Backend.Domain.Models.Posts;
using Backend.Domain.Models.User;
using Backend.Infrastructure.Persistence;
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
                            Type = PostType.Actio,
                            Description =
                                "Meine ältere Nachbarin hat seit ein paar Tagen eine starke Erkältung und wollte eigentlich jemanden bitten, ihr Medikamente zu holen. Als ich sie zufällig im Treppenhaus traf, sah sie wirklich erschöpft aus. Ich habe ihr angeboten, nicht nur Medikamente, sondern gleich den kompletten Wocheneinkauf zu erledigen. Sie gab mir eine kleine Liste, und als ich zurückkam, konnte man ihr richtig ansehen, wie erleichtert sie war. Sie meinte, solche Gesten geben ihr das Gefühl, dass sie noch Teil einer Gemeinschaft ist. Ein schöner Moment."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Nachhilfe für einen überforderten Schüler",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Description = "Ein Nachbarsjunge hatte große Mühe in Mathe und war völlig frustriert. Ich habe mich eine Stunde hingesetzt und mit ihm die Grundlagen wiederholt. Am Ende hat er wirklich verstanden, wie die Aufgabe funktioniert. Seine Augen haben geleuchtet – und er sagte, das sei das erste Mal, dass Mathe ihm nicht Angst macht. Das war ein schönes Gefühl."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Kleines Café-Experiment: Kaffee für die Person hinter mir",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Description =
                                "Ich habe spontan beschlossen, im Café den Kaffee für die Person hinter mir mitzubezahlen. Ich sagte dem Barista, er solle einfach sagen, „heute ist jemand nett gewesen“. Als ich später aus dem Fenster sah, konnte ich beobachten, wie sich die Frau riesig gefreut hat. Einfach schön zu sehen, wie kleine Gesten wirken."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Im Park Müll gesammelt",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Description =
                                "Ich war auf einem kurzen Spaziergang und habe gemerkt, wie viel Müll auf den Wiesen liegt. Ich nahm mir zehn Minuten Zeit, sammelte ein paar Flaschen und Verpackungen auf und war überrascht, wie groß der Unterschied danach schon war. Ein Paar auf einer Bank hat mir sogar zugelächelt und „Danke“ gesagt. Hat sich gut angefühlt."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Beim Umzug spontan ausgeholfen",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Description =
                                "Eine Freundin hatte Schwierigkeiten, ihren Umzug zu stemmen, weil zwei Helfer kurzfristig abgesagt hatten. Ich bin direkt los, obwohl ich selbst eigentlich noch viel zu tun hatte. Am Ende haben wir gemeinsam viel gelacht, und sie meinte, es hätte ohne mich gar nicht geklappt. Diese Art von Freundschaft fühlt sich einfach gut an."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Hund beruhigt, der Angst hatte",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Description =
                                "Ein Hund fing plötzlich an zu winseln und zog panisch an der Leine, während sein Besitzer am Telefon war. Ich hockte mich hin, redete ruhig mit dem Tier und hielt meine Hand hin. Es beruhigte sich nach wenigen Sekunden. Sein Besitzer bedankte sich später – er meinte, er habe selten jemanden erlebt, der so einfühlsam reagiert."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Positiv in Online-Diskussion eingegriffen",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Description =
                                "Eine Diskussion in einem Forum drohte völlig zu eskalieren. Ich habe bewusst sachlich geantwortet und versucht, die Leute wieder auf Augenhöhe zu bringen. Zu meiner Überraschung haben sich gleich zwei Teilnehmer bedankt und meinten, der Ton sei dank meines Kommentars wieder konstruktiver geworden. Manchmal reicht ein einziger Kommentar."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Überstunden übernommen",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Description =
                                "Ein Kollege musste dringend zum Arzt, und er war sichtlich gestresst, weil er seine Schicht nicht einfach verlassen konnte. Ich habe unkompliziert übernommen und er konnte sich um seine Gesundheit kümmern. Am nächsten Tag brachte er mir ein kleines Dankeschön mit. Eine nette Geste, aber ich war vor allem froh, dass ich helfen konnte."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Alten Laptop an Student verschenkt",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Description =
                                "Ich hatte zuhause einen Laptop, den ich schon lange nicht mehr genutzt habe. Ein Student in meinem Umfeld sucht gerade nach einem günstigen Gerät für seine Projektarbeiten. Ich habe ihm meinen einfach geschenkt. Er war so überrascht, dass er kurz sprachlos war. So etwas schenkt nicht nur Technik, sondern auch Motivation."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Fahrkarte für Fremden gekauft",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Description =
                                "Ein Mann konnte sein Ticket nicht kaufen, weil sein Handy abgestürzt war und er kein Kleingeld dabei hatte. Er sah ziemlich verzweifelt aus. Ich habe ihm einfach ein Ticket gekauft. Er sagte, er werde die Geste weitergeben. Ich hoffe, er tut es – solche Kettenreaktionen liebe ich."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Unerwartetes Geschenk: ein Regenschirm",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Description =
                                "Ich wurde vom Regen überrascht, ohne Jacke, ohne Schirm. Eine Frau neben mir öffnete ihren Rucksack, zog einen Ersatzschirm heraus und sagte: „Ich wohne gleich um die Ecke, brauchen Sie ihn?“ Das war so herzlich und selbstverständlich. Ich habe selten so viel Wärme von einem Fremden gespürt."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Jemand hielt mir im perfekten Moment die Tür auf",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Description = "Ich hatte die Hände voller Einkaufstüten, mein Rucksack war halb offen, und ich war schon völlig genervt. Genau in diesem Moment hielt mir jemand die Tür auf und lächelte mich freundlich an. Es war eine Kleinigkeit, aber sie hat meinen ganzen Stress aufgelöst. Manchmal sind es die winzigen Momente, die den Tag retten."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Ein Kompliment, das sitzen blieb",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Description =
                                "Eine Fremde sprach mich an, nur um zu sagen, ich hätte eine positive Ausstrahlung. Ich war völlig überrascht. Der Satz war so ehrlich und klar, dass ich den Rest des Tages lächelnd herumgelaufen bin. Manchmal verändert ein einziger Satz alles."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Starthilfe im richtigen Moment",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Description =
                                "Mein Auto sprang nicht an und ich war schon zu spät. Ein Mann blieb stehen, holte seine Kabel raus und half mir, ohne zu zögern. Er winkte nur und sagte: „Schönen Tag noch!“ – und fuhr davon. Diese Art von Hilfsbereitschaft rührt mich jedes Mal aufs Neue."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Kostenloses Kaffee-Upgrade",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Description =
                                "Ich war müde, gestresst und wollte nur einen kleinen Kaffee. Der Barista lächelte und sagte: „Heute geht’s aufs Haus – nehmen Sie einen großen.“ Dieser winzige Moment der Freundlichkeit hat mich so überrascht, dass ich den restlichen Tag viel leichter nehmen konnte."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Hilfe am Bahnhof",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Description =
                                "Ich hatte zwei schwere Koffer und musste eine steile Treppe hoch. Plötzlich bot mir ein junger Mann an, den schwereren Koffer zu tragen. Oben stellte er ihn ab, wünschte mir eine gute Reise und ging einfach weiter. Ich war ehrlich gerührt."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Nachricht eines alten Freundes",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Description =
                                "Ein Freund aus Schulzeiten schrieb mir völlig spontan, dass er an mich gedacht hat und dankbar für die gemeinsame Jugend ist. Ich hätte niemals erwartet, dass so eine Nachricht mich so sehr berührt. Es hat mich daran erinnert, wie wertvoll menschliche Verbindungen sind."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Sitzplatz angeboten bekommen",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Description =
                                "Ich stand in der überfüllten Bahn und fühlte mich nach einem langen Tag wirklich ausgelaugt. Dann bot mir eine Frau ihren Sitzplatz an und sagte: „Sie sehen aus, als könnten Sie ihn heute besser gebrauchen.“ Ich war sprachlos vor Dankbarkeit."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Kollege bringt Kaffee – kommentarlos",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Description =
                                "Ich kam übermüdet ins Büro. Ein Kollege stellte wortlos einen Kaffee auf meinen Tisch und nickte mir nur zu. Es war ein Moment der stillen Freundlichkeit, der mir unheimlich gutgetan hat."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Portemonnaie zurückbekommen",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Description =
                                "Ich hatte es im Supermarkt liegen gelassen. Eine Frau brachte es mir nach, komplett mit Geld, Karten und allem drum und dran. Ich war so erleichtert und gleichzeitig dankbar für diese Ehrlichkeit. Es hat meinen Glauben an Menschen gestärkt."
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
}
