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
                            Language = "de",
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
                            Language = "de",
                            Description = "Ein Nachbarsjunge hatte große Mühe in Mathe und war völlig frustriert. Ich habe mich eine Stunde hingesetzt und mit ihm die Grundlagen wiederholt. Am Ende hat er wirklich verstanden, wie die Aufgabe funktioniert. Seine Augen haben geleuchtet – und er sagte, das sei das erste Mal, dass Mathe ihm nicht Angst macht. Das war ein schönes Gefühl."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Kleines Café-Experiment: Kaffee für die Person hinter mir",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
                            Description = "Ich hatte die Hände voller Einkaufstüten, mein Rucksack war halb offen, und ich war schon völlig genervt. Genau in diesem Moment hielt mir jemand die Tür auf und lächelte mich freundlich an. Es war eine Kleinigkeit, aber sie hat meinen ganzen Stress aufgelöst. Manchmal sind es die winzigen Momente, die den Tag retten."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Ein Kompliment, das sitzen blieb",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
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
                            Language = "de",
                            Description =
                                "Ich hatte es im Supermarkt liegen gelassen. Eine Frau brachte es mir nach, komplett mit Geld, Karten und allem drum und dran. Ich war so erleichtert und gleichzeitig dankbar für diese Ehrlichkeit. Es hat meinen Glauben an Menschen gestärkt."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Grocery run for my elderly neighbor",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "en",
                            Description =
                                "My elderly neighbor had been sick for a few days and wanted to ask someone to pick up medicine for her, but she felt too weak to go downstairs. I met her in the hallway by chance and she looked exhausted. I offered to get her not just the medication, but her full weekly groceries. When I returned, she seemed deeply relieved. She told me that moments like this remind her she’s still part of a caring community. A small act, but a meaningful one."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Helping a struggling student with math",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "en",
                            Description =
                                "A boy in my neighborhood was completely overwhelmed by his math homework. His frustration was obvious, and he said he felt ‘too dumb’ for the subject. I sat with him for about an hour and went through the basics slowly. By the end of it he understood the concept clearly — his eyes literally lit up. He said it was the first time math didn’t scare him. Seeing that shift in him felt genuinely rewarding."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Paid a coffee for the person behind me",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "en",
                            Description =
                                "At a café, I spontaneously decided to pay for the coffee of the person behind me. I told the barista to simply say, ‘someone wanted to be kind today.’ Later, from outside, I saw the woman’s face when she heard the message — she smiled so warmly that I couldn’t help but smile myself. It’s amazing how tiny gestures can create real moments of joy."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Collected trash in the park during a walk",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "en",
                            Description =
                                "During a short walk in the park, I noticed how much litter was scattered across the grass. I took ten minutes to pick up bottles and food wrappers. The difference afterward was surprisingly big. A couple sitting nearby even smiled and said ‘Thank you.’ It felt good to leave the place a bit better than I found it."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Last-minute moving help",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "en",
                            Description =
                                "A friend of mine was moving apartments, but two of her helpers canceled last minute. She was stressed and overwhelmed, so I rushed over even though I had plenty to do myself. We worked hard but laughed even harder. She said she couldn’t have managed without me. Helping in moments like these always reminds me of the kind of friend I want to be."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Calming a frightened dog",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "en",
                            Description =
                                "A dog suddenly started whining and pulling in panic while its owner was on the phone. I crouched down, talked softly, and held my hand out. Within seconds, the dog relaxed. The owner thanked me afterward and said he rarely sees people respond with such calmness. It was a sweet, quiet moment of connection."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Defusing a heated online discussion",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "en",
                            Description =
                                "An online discussion was spiraling into insults and hostility. I replied calmly, summarized the actual topic, and reminded everyone to respect each other. To my surprise, two participants thanked me, saying the conversation became constructive again because of my comment. It’s nice to see that one thoughtful message can shift the tone for everyone."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Covered a colleague’s shift",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "en",
                            Description =
                                "A coworker had a medical appointment he couldn’t miss, but he was stressed because he couldn’t leave his shift unattended. I stepped in for him without hesitation. The next day he brought me a small thank-you gift. His relief meant more to me than the gift — it felt good to make someone’s day easier."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Gave my old laptop to a student",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "en",
                            Description =
                                "I had an old laptop lying around that I barely used anymore. A student I knew needed a device for his university projects but couldn’t afford one. I gave him mine. He was so surprised he didn’t know what to say at first. Moments like that show how much impact unused things can still have for someone else."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Bought a train ticket for a stranger",
                            UserId = appUser.Id,
                            Type = PostType.Actio,
                            Language = "en",
                            Description =
                                "A man couldn’t buy a ticket because his phone had crashed and he had no cash on him. He looked genuinely desperate, so I simply bought the ticket for him. He said he would ‘pass the kindness on.’ I really hope he does — I love seeing kindness ripple forward."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Given an umbrella by a stranger",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "en",
                            Description =
                                "I was caught in the rain with no jacket and no umbrella. A woman next to me opened her backpack, pulled out a spare umbrella, and asked, ‘Do you need one? I live just around the corner.’ Her kindness was so natural and warm. Moments like this remind me that compassion often comes from complete strangers."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Door held open at the perfect moment",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "en",
                            Description =
                                "I had my hands full of shopping bags, and my backpack was half open. I was stressed and annoyed — then someone held the door for me with a friendly smile. It was such a small act, but it instantly lifted the weight of the moment. Sometimes tiny gestures fix an entire day."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "A compliment that stayed with me",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "en",
                            Description =
                                "A stranger approached me just to say I had a positive energy about me. It was unexpected and sincere. That one sentence stayed with me all day — it’s incredible how a few kind words can shift your whole mood."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Jump-start from a passerby",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "en",
                            Description =
                                "My car wouldn’t start and I was already late. A man stopped, grabbed jumper cables from his trunk, and helped me without hesitation. He just waved goodbye and wished me a good day. His spontaneous kindness genuinely moved me."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Free coffee upgrade",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "en",
                            Description =
                                "I was exhausted and ordered a small coffee. The barista smiled and said, ‘You look like you need a large today — on the house.’ It was unexpected, simple, and incredibly uplifting. It changed the tone of my entire morning."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Help with my luggage at the station",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "en",
                            Description =
                                "I was struggling up a steep staircase with two heavy suitcases when a young man offered to carry the heavier one. He placed it at the top, wished me a safe trip, and walked away. His kindness touched me more than he probably realized."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Unexpected message from an old friend",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "en",
                            Description =
                                "A friend from my school days messaged me out of the blue to say he had been thinking about me and was grateful for the time we spent growing up together. I never expected such a message to affect me so deeply. It reminded me how valuable old connections really are."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Offered a seat on a crowded train",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "en",
                            Description =
                                "I was exhausted after a long day, standing in a completely packed train. A woman offered me her seat and said, ‘You look like you need it more today.’ The gesture was so thoughtful that I was almost speechless."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Coworker brought me coffee without asking",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "en",
                            Description =
                                "I walked into the office tired and unfocused. A colleague quietly placed a coffee on my desk and gave me a small nod. No words, just kindness. That simple gesture made my whole morning easier."
                        },
                        new Post()
                        {
                            Id = new Guid(),
                            CreatedAt = DateTime.Now,
                            Title = "Wallet returned — with everything inside",
                            UserId = appUser.Id,
                            Type = PostType.Reactio,
                            Language = "en",
                            Description =
                                "I had left my wallet in the supermarket. A woman brought it to me shortly afterward — with all the money, cards, and documents untouched. I felt an enormous wave of relief and gratitude. Her honesty restored my faith in humanity."
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
