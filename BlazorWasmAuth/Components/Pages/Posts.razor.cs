using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.DTOs.Posts;

namespace Frontend.Components.Pages;

public partial class Posts
{
    private readonly List<PostDto> posts = new();
    private bool _isLoading;
    private bool _allLoaded;
    private int _page;
    private const int PageSize = 10;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync(
                "registerScrollHandler",
                DotNetObjectReference.Create(this));

            await LoadMoreAsync();
        }
    }

    [JSInvokable]
    public async Task OnWindowScroll()
    {
        var info = await JS.InvokeAsync<ScrollInfo>("getScrollInfo");

        if (!_isLoading && !_allLoaded &&
            info.scrollTop + info.windowHeight >= info.scrollHeight - 200)
        {
            await LoadMoreAsync();
        }
    }

    public record ScrollInfo(double scrollTop, double windowHeight, double scrollHeight);

    private async Task LoadMoreAsync()
    {
        try
        {
            _isLoading = true;
            StateHasChanged();

            var response = await ServiceClient.GetPagedPosts(_page, PageSize);

            if (response?.Items?.Count > 0)
            {
                posts.AddRange(response.Items);
                _page++;
            }

            if (response == null || !response.HasMore)
            {
                _allLoaded = true;
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error during loading: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    [Inject] private IJSRuntime JS { get; set; } = default!;
}