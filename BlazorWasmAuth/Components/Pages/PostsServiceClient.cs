using Shared.DTOs.Posts;
using System.Net.Http;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Frontend.Components.Pages
{
    internal class PostsServiceClient
    {
        private readonly HttpClient _httpClient;

        public PostsServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
        }

        public async Task<PagedResponse<PostDto>?> GetPagedPosts(int page, int pageSize)
        {
            return await _httpClient.GetFromJsonAsync<PagedResponse<PostDto>>(
                $"api/posts/paged?page={page}&pageSize={pageSize}");
        }
    }
}
