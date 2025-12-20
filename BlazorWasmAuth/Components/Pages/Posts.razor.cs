using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.DTOs.Posts;

namespace Frontend.Components.Pages;

public partial class Posts : IDisposable
{
    private readonly List<PostDto> posts = new();
    private bool _isLoading;
    private bool _allLoaded;
    private int _page;
    private const int PageSize = 10;

    private double _pullDistance = 0;
    private bool _isRefreshing = false;

    [Parameter]
    public PostType PostType { get; set; }

    protected override void OnInitialized()
    {
        FooterService.Hide();
    }

    public void Dispose()
    {
        FooterService.Show();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("registerScrollHandler", DotNetObjectReference.Create(this));
            await JS.InvokeVoidAsync("registerPullToRefresh", DotNetObjectReference.Create(this));

            await LoadMoreAsync();
        }
    }

    [JSInvokable]
    public async Task OnWindowScroll(ScrollInfo info)
    {
        if (!_isLoading && !_allLoaded && info.scrollTop + info.windowHeight >= info.scrollHeight - 200)
        {
            await LoadMoreAsync();
        }
    }

    [JSInvokable]
    public Task OnPullProgress(double distance)
    {
        _pullDistance = distance;
        StateHasChanged();
        return Task.CompletedTask;
    }

    [JSInvokable]
    public async Task OnPullTriggered()
    {
        _isRefreshing = true;
        _pullDistance = 0;
        StateHasChanged();

        posts.Clear();
        _page = 0;
        _allLoaded = false;

        await LoadMoreAsync();

        _isRefreshing = false;
        StateHasChanged();
    }

    public record ScrollInfo(double scrollTop, double windowHeight, double scrollHeight);

    private async Task LoadMoreAsync()
    {
        try
        {
            _isLoading = true;
            StateHasChanged();

            var response = await ServiceClient.GetPagedPosts(_page, PageSize, PostType);

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