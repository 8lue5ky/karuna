namespace Frontend.Components.Pages.Comments
{
    using Shared.DTOs.Comments;
    using System.Net.Http;
    using System.Net.Http.Json;

    public class CommentServiceClient
    {
        private readonly HttpClient _httpClient;

        public CommentServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
        }

        public async Task<List<CommentDto>> GetCommentsAsync(Guid postId)
        {
            return await _httpClient.GetFromJsonAsync<List<CommentDto>>(
                $"api/comments/post/{postId}"
            ) ?? new();
        }

        public async Task<Guid?> CreateAsync(CreateCommentDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/comments", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            var body = await response.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();

            return body?["id"];
        }

        public async Task<bool> DeleteAsync(Guid commentId)
        {
            var response = await _httpClient.DeleteAsync($"api/comments/{commentId}");
            return response.IsSuccessStatusCode;
        }
    }

}
