using Shared.DTOs.Posts;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Frontend.Components.Pages
{
    public class PostsServiceClient
    {
        private readonly HttpClient _httpClient;

        public PostsServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
        }

        public async Task<PagedResponse<PostDto>?> GetPagedPosts(int page, int pageSize, PostType postType)
        {
            return await _httpClient.GetFromJsonAsync<PagedResponse<PostDto>>(
                $"api/posts/paged?page={page}&pageSize={pageSize}&type={postType}");
        }

        public async Task<HttpResponseMessage> CreatePostAsync(CreatePost.PostModel post)
        {
            return await _httpClient.PostAsJsonAsync("api/posts", post);
        }

        public async Task LikePostAsync(Guid postId)
        {
            await _httpClient.PostAsync($"api/posts/{postId}/like", null);
        }

        public async Task UnLikePostAsync(Guid postId)
        {
            await _httpClient.DeleteAsync($"api/posts/{postId}/like");
        }

        public async Task<PostDto?> GetPost(Guid postId)
        {
            return await _httpClient.GetFromJsonAsync<PostDto>($"api/posts/{postId}");
        }
    }
}
