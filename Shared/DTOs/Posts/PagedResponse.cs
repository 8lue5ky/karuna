namespace Shared.DTOs.Posts;

public class PagedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public bool HasMore { get; set; }
}