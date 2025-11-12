using System.ComponentModel.DataAnnotations;

namespace Backend.Controller.Posts;

public class PostCreateRequest
{
    [Required(ErrorMessage = "A title is required.")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "A description is required.")]
    public required string Description { get; set; }
}