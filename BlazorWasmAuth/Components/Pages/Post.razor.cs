using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.DTOs.Posts;

namespace Frontend.Components.Pages;


public partial class Post
{
    [Parameter] 
    public required PostDto PostModel { get; set; }

    [Inject]
    public required PostsServiceClient PostsServiceClient { get; set; }

    public string TimeAgo => PostModel.CreatedAt.ToTimeAgo(Localizer);

    public bool IsExamplePost => PostModel.Username == "Harry" || PostModel.Username == "Leela";

    public bool IsAnonymous => PostModel.UserId == null;

    public string UserName => PostModel.Username ?? Localizer["AnonymousUser"];

    private string GetThumbnailUrl()
    {
        var backendUrl = Configuration["BackendUrl"];

        if (IsAnonymous)
        {
            return $"{backendUrl}/images/anonymous_profile.png";
        }

        return $"{backendUrl}/uploads/users/{PostModel.UserId}/profile.png";
    }

    private void GoToDetails()
    {
        NavigationManager.NavigateTo($"/post/{PostModel.Id}");
    }

    private string GetLikeIcon()
    {
        return PostModel.HasLiked == true
            ? Icons.Material.Filled.Favorite
            : Icons.Material.Outlined.FavoriteBorder;
    }

    private Color GetLikeColor()
    {
        return PostModel.HasLiked == true 
            ? Color.Error 
            : Color.Default;
    }

    private async Task ToggleLike()
    {
        if (PostModel.HasLiked == true)
        {
            await PostsServiceClient.UnLikePostAsync(PostModel.Id);

            PostModel.HasLiked = false;
            PostModel.LikeCount--;
        }
        else
        {
            await PostsServiceClient.LikePostAsync(PostModel.Id);

            PostModel.HasLiked = true;
            PostModel.LikeCount++;
        }

        StateHasChanged();
    }
}