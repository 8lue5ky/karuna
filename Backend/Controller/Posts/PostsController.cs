using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Models.Posts;
using Backend.Domain.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            string language = Request.GetBestMatchingUserLanguage();

            Post post = new Post()
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                Type = request.Type == PostTypeDto.Actio ? PostType.Actio : PostType.Reactio,
                Id = Guid.NewGuid(),
                Title = request.Title,
                Language = language,
                Description = request.Description,
            };

            _postsRepository.CreatePostAsync(post);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(Guid id)
        {
            PostDto? post = await _postsRepository.GetPostAsync(id);

            if (post == null) return NotFound();

            return post;
        }

        [AllowAnonymous]
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResponse<PostDto>>> GetPaged([FromQuery] int page = 0, [FromQuery] int pageSize = 10, [FromQuery] PostTypeDto type = PostTypeDto.Actio)
        {
            if (page < 0 || pageSize <= 0)
            {
                return BadRequest("Invalid parameters.");
            }

            var userId = await GetUserIdIfLoggedIn();
            string language = Request.GetBestMatchingUserLanguage();

            var skip = page * pageSize;

            var postType = type == PostTypeDto.Actio ? PostType.Actio : PostType.Reactio;
            var posts = await _postsRepository.GetPostsAsyncOrderedByCreated(pageSize, skip,  postType, language, userId);


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
