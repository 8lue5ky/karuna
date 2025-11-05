using System.ComponentModel.DataAnnotations;

namespace Backend.Controller;

public class GoodDeedCreateRequest
{
    [Required(ErrorMessage = "A title is required.")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "A description is required.")]
    public string? Description { get; set; }
}