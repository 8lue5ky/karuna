using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.DTOs.Posts;

namespace Frontend.Components.Pages;

public partial class GoodDeeds
{
    private readonly List<PostDto> _goodDeedPosts = new();
    private bool _isLoading;
    private bool _allLoaded;
    private ElementReference _scrollDiv;
    private int _page;
    private const int PageSize = 10;

    protected override async Task OnInitializedAsync()
    {
        await LoadMoreAsync();
    }

    private async Task OnScroll(EventArgs e)
    {
        var scrollPos = await JS.InvokeAsync<double>("getScrollPosition", _scrollDiv);
        var scrollHeight = await JS.InvokeAsync<double>("getScrollHeight", _scrollDiv);
        var clientHeight = await JS.InvokeAsync<double>("getClientHeight", _scrollDiv);

        if (!_isLoading && !_allLoaded && scrollPos + clientHeight >= scrollHeight - 200)
        {
            await LoadMoreAsync();
        }
    }

    private async Task LoadMoreAsync()
    {
        try
        {
            _isLoading = true;
            StateHasChanged();

            var response = await ServiceClient.GetPagedPosts(_page, PageSize);

            if (response?.Items?.Count > 0)
            {
                _goodDeedPosts.AddRange(response.Items);
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