using Backend.Models;
using Backend.Models.User;
using Backend.Persistence.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Posts;

namespace Backend.Controller.Posts
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IPostsRepository _postsRepository;
        private readonly UserManager<AppUser> _userManager;

        public PostsController(ILogger<PostsController> logger, IPostsRepository postsRepository, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _postsRepository = postsRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// POST /api/posts
        /// </summary>
        [Authorize]
        [HttpPost]
        public IActionResult CreatePost([FromBody] PostCreateRequest request)
        {
            string? userId = _userManager.GetUserId(User);

            if (!ModelState.IsValid || userId == null)
            {
                return BadRequest(ModelState);
            }

            Post post = new Post()
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description
            };

            _postsRepository.CreatePostAsync(post);

            return Ok();
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResponse<PostDto>>> GetPaged([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            if (page < 0 || pageSize <= 0)
            {
                return BadRequest("Invalid parameters.");
            }

            var skip = page * pageSize;

            var posts = await _postsRepository.GetPostsAsyncOrderedByCreated(pageSize, skip);

            List<PostDto> postDtos = posts.Posts.Select(x => new PostDto()
            {
                CreatedAt = x.CreatedAt,
                Description = x.Description,
                Id = x.Id,
                Title = x.Title,
                UserId = x.UserId,
                Username = x.User.DisplayName
            }).ToList();

            return Ok(new PagedResponse<PostDto>
            {
                Items = postDtos,
                HasMore = posts.HasMore
            });
        }
    }
}
