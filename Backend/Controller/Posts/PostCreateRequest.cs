using System.ComponentModel.DataAnnotations;

namespace Backend.Controller.Posts;

public class PostCreateRequest
{
    [Required(ErrorMessage = "A title is required.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "A description is required.")]
    public string? Description { get; set; }
}