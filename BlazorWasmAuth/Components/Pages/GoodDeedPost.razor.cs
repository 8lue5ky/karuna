using Microsoft.AspNetCore.Components;

namespace Frontend.Components.Pages;

public partial class GoodDeedPost
{
    [Parameter] public string Username { get; set; } = "GoodDeedHero";
    [Parameter] public string TimeAgo { get; set; } = "5 minutes ago";
    [Parameter] public string Title { get; set; } = "🌟 A good deed!";

    [Parameter]
    public string Content { get; set; } =
        "Today I helped an elderly lady carry her shopping home.";

    [Parameter] public int Likes { get; set; } = 23;
    [Parameter] public int Comments { get; set; } = 4;

    [Parameter] public string UserId { get; set; }

    private string GetThumbnailUrl()
    {
        var backendUrl = Configuration["BackendUrl"];
        return $"{backendUrl}/api/profile/{UserId}/thumbnail";
    }
}