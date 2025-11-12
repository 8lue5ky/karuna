using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Posts;

namespace Backend.Controller.Posts
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;

        private static readonly List<PostDto> _posts = Enumerable.Repeat(new PostDto()
        {
            Title = "Good deed",
            Description = "LoremIpsum"
        }, 20).ToList();

        public PostsController(ILogger<PostsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// POST /api/posts
        /// </summary>
        [HttpPost]
        public IActionResult CreatePost([FromBody] PostCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deed = new PostDto
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };

            _posts.Add(deed);

            return Ok();
        }

        [HttpGet("paged")]
        public ActionResult<PagedResponse<PostDto>> GetPaged([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            if (page < 0 || pageSize <= 0)
            {
                return BadRequest("Invalid parameters.");
            }

            var skip = page * pageSize;

            var items = _posts
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            var hasMore = skip + pageSize < _posts.Count;

            return Ok(new PagedResponse<PostDto>
            {
                Items = items,
                HasMore = hasMore
            });
        }
    }
}
