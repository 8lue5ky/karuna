namespace Shared.DTOs.Posts;

public class PagedResponse<T>
{
    public IReadOnlyList<T> Items { get; set; } = new List<T>();
    public bool HasMore { get; set; }
}