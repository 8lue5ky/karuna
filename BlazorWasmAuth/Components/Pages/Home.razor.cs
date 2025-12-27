using Microsoft.AspNetCore.Components;

namespace Frontend.Components.Pages;

public partial class Home
{
    private int _activeTabIndex;
    private bool _loaded = false;

    [Inject]
    private PostsServiceClient _serviceClient { get; set; }

    protected override async Task OnInitializedAsync()
    {
        PostType result = await _serviceClient.GetNewestPostCategoryAsync();

        _activeTabIndex = result == PostType.Reactio ? 1 : 0;

        _loaded = true;
    }
}