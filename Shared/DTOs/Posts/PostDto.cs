namespace Shared.DTOs.Posts;

public class PostDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Username { get; set; }
    public string? UserId { get; set; }

    public DateTime CreatedAt { get; set; }
}