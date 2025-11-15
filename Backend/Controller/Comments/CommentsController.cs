using Backend.Persistence.Comments;
using Microsoft.AspNetCore.Authorization;
using Shared.DTOs.Comments;

namespace Backend.Controller.Comments
{
    using Backend.Models.Posts;
    using Backend.Models.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly UserManager<AppUser> _userManager;

        public CommentsController(ICommentsRepository commentsRepository, UserManager<AppUser> userManager)
        {
            _commentsRepository = commentsRepository;
            _userManager = userManager;
        }

        // ------------------------------------------------------------
        // POST: api/comments
        // Create comment (requires login)
        // ------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto dto)
        {
            string? userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Unauthorized();
            }

            var comment = new Comment
            {
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                PostId = dto.PostId,
                UserId = userId
            };

            await _commentsRepository.CreateCommentAsync(comment);

            return Ok(new { comment.Id });
        }

        [AllowAnonymous]
        // ------------------------------------------------------------
        // GET: api/comments/post/{postId}
        // Load comments for a post (public)
        // ------------------------------------------------------------
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetComments(Guid postId)
        {
            var comments = await _commentsRepository.GetCommentsAsync(postId);

            return Ok(comments);
        }

        // ------------------------------------------------------------
        // DELETE: api/comments/{id}
        // Delete comment (require login + author)
        // ------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            string? userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Unauthorized();
            }

            await _commentsRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
