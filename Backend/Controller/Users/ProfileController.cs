using Backend.Models.User;
using Backend.Persistence.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.User;

namespace Backend.Controller.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ProfileController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;

        public ProfileController(IWebHostEnvironment env, ILogger<ProfileController> logger, UserManager<AppUser> userManager, IUserRepository userRepository)
        {
            _env = env;
            _logger = logger;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile()
        {
            string? userId = _userManager.GetUserId(User);

            if (userId != null)
            {
                var userProfile = await _userRepository.GetUserProfileAsync(userId);

                if (userProfile != null)
                {
                    return Ok(new UserProfileDto()
                    {
                        DisplayName = userProfile.User.DisplayName,
                        Bio = userProfile.Bio,
                        Email = userProfile.User.Email,
                        Id = userProfile.Id,
                        Location = userProfile.Location
                    });
                }
            }

            return NotFound();
        }

        [Authorize]
        [HttpPost("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string? userId = _userManager.GetUserId(User);

                byte[]? imageData = null;

                if (dto.ProfileImage is not null && dto.ProfileImage.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await dto.ProfileImage.CopyToAsync(ms);
                    imageData = ms.ToArray();
                }

                var action = new UpdateProfileAction()
                {
                    ProfilePicture = imageData,
                    DisplayName = dto.Username,
                    Email = dto.Email,
                    Bio = dto.Bio,
                    Location = dto.Location
                };

                await _userRepository.UpdateProfileAsync(userId, action);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during updating user profile.");
                return StatusCode(500);
            }
        }

        [HttpGet("{userId}/thumbnail")]
        public async Task<IActionResult> GetThumbnail(string userId)
        {
            byte[]? thumbnail = await _userRepository.GetProfileImageThumbnailAsync(userId);

            if (thumbnail == null)
            {
                var redirectUrl = "https://cdn-icons-png.flaticon.com/512/847/847969.png";
                return Redirect(redirectUrl);
            }

            return File(thumbnail, "image/jpeg");
        }

        [Authorize]
        [HttpGet("thumbnail")]
        public async Task<IActionResult> GetThumbnail()
        {
            string? userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                var redirectUrl = "https://cdn-icons-png.flaticon.com/512/847/847969.png";
                return Redirect(redirectUrl);
            }

            return await GetThumbnail(userId);
        }



        //private byte[] GenerateThumbnail(byte[] imageBytes, int width, int height)
        //{
        //    using var inputStream = new MemoryStream(imageBytes);
        //    using var image = System.Drawing.Image.FromStream(inputStream);

        //    using var thumbnail = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
        //    using var ms = new MemoryStream();
        //    thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    return ms.ToArray();
        //}
    }
}
