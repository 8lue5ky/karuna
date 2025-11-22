using Backend.Models.User;
using Backend.Persistence.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.DTOs.User;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                        DisplayName = userProfile.User.UserName!,
                        Bio = userProfile.Bio,
                        Email = userProfile.User.Email,
                        UserId = userProfile.UserId,
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


                var action = new UpdateProfileAction()
                {
                    ProfilePicture = imageData,
                    DisplayName = dto.Username,
                    Email = dto.Email,
                    Bio = dto.Bio,
                    Location = dto.Location
                };

                var result = await _userRepository.UpdateProfileAsync(userId, action);

                if (!result.Succeeded)
                {
                    var modelState = new ModelStateDictionary();

                    foreach (var error in result.Errors)
                    {
                        modelState.AddModelError(error.Code, error.Description);
                    }

                    var details = new ValidationProblemDetails(modelState)
                    {
                        Title = "Registration failed.",
                        Status = StatusCodes.Status400BadRequest
                    };

                    return BadRequest(details);
                }

                if (dto.ProfileImage is not null && dto.ProfileImage.Length > 0)
                {
                    await SaveImage(dto, userId);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during updating user profile.");
                return StatusCode(500);
            }
        }

        private static async Task SaveImage(ProfileUpdateDto dto, string? userId)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "users", userId);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, "profile.png");

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.ProfileImage.CopyToAsync(stream);
        }
    }
}
