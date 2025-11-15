using Backend.Models.Posts;
using Backend.Models.User;
using Backend.Persistence.Posts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Posts;

namespace Backend.Controller.Posts
{
    [Authorize]
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

        [AllowAnonymous]
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResponse<PostDto>>> GetPaged([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            if (page < 0 || pageSize <= 0)
            {
                return BadRequest("Invalid parameters.");
            }

            var userId = await GetUserIdIfLoggedIn();

            var skip = page * pageSize;

            var posts = await _postsRepository.GetPostsAsyncOrderedByCreated(pageSize, skip, userId);


            return Ok(new PagedResponse<PostDto>
            {
                Items = posts.Posts,
                HasMore = posts.HasMore
            });
        }

        [HttpPost("{postId}/like")]
        public async Task<IActionResult> LikePost(Guid postId)
        {
            string? userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Unauthorized();
            }

            await _postsRepository.LikePostAsync(postId, userId);

            return Ok();
        }

        [HttpDelete("{postId}/like")]
        public async Task<IActionResult> UnlikePost(Guid postId)
        {
            string? userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Unauthorized();
            }

            await _postsRepository.UnlikePostAsync(postId, userId);

            return Ok();
        }

        private async Task<string?> GetUserIdIfLoggedIn()
        {
            var auth = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

            string? userId = auth.Succeeded
                ? _userManager.GetUserId(auth.Principal)
                : null;

            return userId;
        }
    }
}
